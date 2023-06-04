using AutoMapper;
using Core.Enumerations;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Data.Extensions;
using Microsoft.Extensions.Logging;
using Services.Dtos;
using Services.Search;

namespace Services.Management
{
    public class AnimalVisitedLocationManagementService : IAnimalVisitedLocationManagementService
    {
        readonly ILogger<AnimalVisitedLocationManagementService> _logger;
        readonly ChippedAnimalsDbContext _context;
        readonly IAnimalVisitedLocationSearchService _animalVisitedLocationSearchService;
        readonly IMapper _mapper;

        public AnimalVisitedLocationManagementService(
            ILogger<AnimalVisitedLocationManagementService> logger,
            ChippedAnimalsDbContext context,
            IAnimalVisitedLocationSearchService animalVisitedLocationSearchService,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _animalVisitedLocationSearchService = animalVisitedLocationSearchService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AnimalVisitedLocationDto>> GetListByFiltersAsync(
            long? animalId, AnimalVisitedLocationListDto listDto)
        {
            IEnumerable<AnimalVisitedLocation> fetchedVisitedLocations =
                await _animalVisitedLocationSearchService
                    .FindListByFiltersAsync(animalId, listDto);
            _logger.LogInformation("Fetched from database {@models}", fetchedVisitedLocations);
            return _mapper.Map<IEnumerable<AnimalVisitedLocationDto>>(fetchedVisitedLocations);
        }

        public async Task<AnimalVisitedLocationDto> CreateAsync(long? animalId, long? pointId)
        {
            if (!await DoesLocationExistsAsync(pointId))
            {
                throw new LocationNotFoundException(pointId);
            }
            Animal? fetchedAnimal = await _context.Animals.FetchByIdAsync(animalId);
            if (fetchedAnimal == null)
            {
                throw new AnimalNotFoundException(animalId);
            }
            if (IsDead(fetchedAnimal))
            {
                throw new AnimalIsDeadException(animalId);
            }
            if (IsSameAsCurrentLocation(pointId, fetchedAnimal))
            {
                throw new AnimalAlreadyInLocation(animalId, pointId);
            }
            AnimalVisitedLocation newVisitedLocation = 
                await AddVisitedLocationAsync(animalId, pointId);
            await _context.SaveChangesAsync();
            return _mapper.Map<AnimalVisitedLocationDto>(newVisitedLocation);
        }

        public async Task<AnimalVisitedLocationDto> UpdateAsync(
            long? animalId, AnimalVisitedLocationUpdateDto updateDto)
        {
            if (!await DoesLocationExistsAsync(updateDto.LocationId))
            {
                throw new LocationNotFoundException(updateDto.LocationId);
            }
            AnimalVisitedLocation? fetchedVisitedLocation = await _context.AnimalVisitedLocations
                .FindAsync(updateDto.VisitedLocationId);
            _logger.LogInformation("Fetched from database {@model}", fetchedVisitedLocation);
            if (fetchedVisitedLocation == null)
            {
                throw new AnimalVisitedLocationNotFoundException(updateDto.VisitedLocationId);
            }
            if (fetchedVisitedLocation.LocationId == updateDto.LocationId)
            {
                throw new OldLocationSameAsNewException(updateDto.LocationId);
            }
            Animal? fetchedAnimal = await _context.Animals.FetchByIdAsync(animalId);
            _logger.LogInformation("Fetched from database {@model}", fetchedAnimal);
            if (fetchedAnimal == null)
            {
                throw new AnimalNotFoundException(animalId);
            }
            if (!DoesAnimalHasVisitedLocation(fetchedAnimal, updateDto.VisitedLocationId))
            {
                throw new AnimalDoesNotHaveVisitedLocationException(
                    animalId, updateDto.VisitedLocationId);
            }
            if (IsSameAsNextOrPreviousLocation(
                updateDto.LocationId, fetchedVisitedLocation, fetchedAnimal))
            {
                throw new AnimalVisitedLocationSameAsNextOrPreviousException(
                    updateDto.VisitedLocationId);
            }
            _mapper.Map(updateDto, fetchedVisitedLocation);
            await _context.SaveChangesAsync();
            return _mapper.Map<AnimalVisitedLocationDto>(fetchedVisitedLocation);
        }

        public async Task DeleteAsync(long? animalId, long? visitedPointId)
        {
            Animal? fetchedAnimal = await _context.Animals.FetchByIdAsync(animalId);
            _logger.LogInformation("Fetched from database {@model}", fetchedAnimal);
            if (fetchedAnimal == null)
            {
                throw new AnimalNotFoundException(animalId);
            }
            AnimalVisitedLocation? fetchedVisitedLocation = await _context.AnimalVisitedLocations
                .FindAsync(visitedPointId);
            _logger.LogInformation("Fetched from database {@model}", fetchedVisitedLocation);
            if (fetchedVisitedLocation == null)
            {
                throw new AnimalVisitedLocationNotFoundException(visitedPointId);
            }
            if (!DoesAnimalHasVisitedLocation(fetchedAnimal, visitedPointId))
            {
                throw new AnimalDoesNotHaveVisitedLocationException(animalId, visitedPointId);
            }
            RemoveVisitedLocation(fetchedVisitedLocation, fetchedAnimal);
            await _context.SaveChangesAsync();
        }

        async Task<AnimalVisitedLocation> AddVisitedLocationAsync(long? animalId, long? pointId)
        {
            AnimalVisitedLocation newVisitedLocation = new AnimalVisitedLocation
            {
                VisitDateTime = DateTime.UtcNow,
                LocationId = pointId!.Value,
                AnimalId = animalId!.Value
            };
            await _context.AnimalVisitedLocations.AddAsync(newVisitedLocation);
            return newVisitedLocation;
        }

        void RemoveVisitedLocation(AnimalVisitedLocation visitedLocation, Animal associatedAnimal)
        {
            IList<AnimalVisitedLocation> orderedVisitedLocations =
                GetVisitedLocationsOrderedByVisitDateTime(associatedAnimal);
            _context.AnimalVisitedLocations.Remove(visitedLocation);
            orderedVisitedLocations.Remove(visitedLocation);
            AnimalVisitedLocation? nextVisitedLocation = orderedVisitedLocations.FirstOrDefault();
            if (associatedAnimal.ChippingLocationId == nextVisitedLocation?.LocationId)
            {
                _context.AnimalVisitedLocations.Remove(nextVisitedLocation);
                associatedAnimal.VisitedLocations.Remove(nextVisitedLocation);
            }
        }

        bool IsSameAsCurrentLocation(long? newLocationId, Animal associatedAnimal)
        {
            IList<AnimalVisitedLocation> orderedVisitedLocations = 
                GetVisitedLocationsOrderedByVisitDateTime(associatedAnimal);
            return orderedVisitedLocations.Any()
                ? orderedVisitedLocations.Last().LocationId == newLocationId
                : associatedAnimal.ChippingLocationId == newLocationId;
        }

        bool IsSameAsNextOrPreviousLocation(long? newLocationId,
            AnimalVisitedLocation updatedVisitedLocation, Animal associatedAnimal)
        {
            IList<AnimalVisitedLocation> orderedVisitedLocations = 
                GetVisitedLocationsOrderedByVisitDateTime(associatedAnimal);
            int currentVisitedLocationIndex = orderedVisitedLocations
                .IndexOf(updatedVisitedLocation);
            bool result = false;
            if (currentVisitedLocationIndex - 1 > -1)
            {
                result |= orderedVisitedLocations[currentVisitedLocationIndex - 1].LocationId
                    == newLocationId;
            }
            else
            {
                result |= associatedAnimal.ChippingLocationId == newLocationId;
            }
            if (currentVisitedLocationIndex + 1 < orderedVisitedLocations.Count)
            {
                result |= orderedVisitedLocations[currentVisitedLocationIndex + 1].LocationId
                    == newLocationId;
            }
            return result;
        }

        async Task<bool> DoesLocationExistsAsync(long? pointId)
        {
            return await _context.Locations.DoesExistsAsync(pointId);
        }

        bool IsDead(Animal fetchedAnimal)
        {
            return ParseLifeStatus(fetchedAnimal.LifeStatus.LifeStatus) == LifeStatus.Dead;
        }

        bool DoesAnimalHasVisitedLocation(Animal fetchedAnimal, long? visitedLocationId)
        {
            return fetchedAnimal.VisitedLocations.Any(avl => avl.Id == visitedLocationId);
        }

        List<AnimalVisitedLocation> GetVisitedLocationsOrderedByVisitDateTime(Animal associatedAnimal)
        {
            return associatedAnimal.VisitedLocations
                            .OrderBy(avl => avl.VisitDateTime)
                            .ToList();
        }

        LifeStatus? ParseLifeStatus(string lifeStatus)
        {
            Enum.TryParse(typeof(LifeStatus), lifeStatus, true, out object? result);
            return (LifeStatus?)result;
        }
    }
}
