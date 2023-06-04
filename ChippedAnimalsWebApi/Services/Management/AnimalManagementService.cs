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
    public class AnimalManagementService : IAnimalManagementService
    {
        readonly ILogger<AnimalManagementService> _logger;
        readonly ChippedAnimalsDbContext _context;
        readonly IAnimalSearchService _animalSearchService;
        readonly IMapper _mapper;

        public AnimalManagementService(
            ILogger<AnimalManagementService> logger,
            ChippedAnimalsDbContext context,
            IAnimalSearchService animalSearchService,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _animalSearchService = animalSearchService;
            _mapper = mapper;
        }

        public async Task<AnimalDto> GetByIdAsync(long? animalId)
        {
            Animal? fetchedAnimal = await _context.Animals.FetchByIdNoTrackingAsync(animalId);
            _logger.LogInformation("Fetched from database {@model}", fetchedAnimal);
            if (fetchedAnimal == null)
            {
                throw new AnimalNotFoundException(animalId);
            }
            return _mapper.Map<AnimalDto>(fetchedAnimal);
        }

        public async Task<IEnumerable<AnimalDto>> GetListByFiltersAsync(AnimalListDto listDto)
        {
            IEnumerable<Animal> fetchedAnimals = await _animalSearchService
                .FindListByFiltersAsync(listDto);
            _logger.LogInformation("Fetched from database {@models}", fetchedAnimals);
            return _mapper.Map<IEnumerable<AnimalDto>>(fetchedAnimals);
        }

        public async Task<AnimalDto> CreateAsync(AnimalCreateDto createDto)
        {
            if (!await DoesChipperExistsAsync(createDto.ChipperId))
            {
                throw new AccountNotFoundException(createDto.ChipperId);
            }
            if (!await DoesChippingLocationExistsAsync(createDto.ChippingLocationId))
            {
                throw new ChippingLocationNotFoundException(createDto.ChippingLocationId);
            }
            AnimalGender? gender = await _context.AnimalGenders.FetchByNameAsync(createDto.Gender);
            if (gender == null)
            {
                throw new GenderNameNotExistsException(createDto.Gender);
            }
            IEnumerable<long> nonExistentTypeIds = await _context.AnimalTypes
                .SelectNonExistentIdsAsync(createDto.Types);
            if (nonExistentTypeIds.Any())
            {
                throw new AnimalTypesNotExistException(nonExistentTypeIds);
            }
            Animal newAnimal = await AddAnimalAsync(createDto, gender!);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Added to database: {@model}", newAnimal);
            return _mapper.Map<AnimalDto>(newAnimal);
        }

        public async Task<AnimalDto> UpdateAsync(long? animalId, AnimalUpdateDto updateDto)
        {
            AnimalGender? gender = await _context.AnimalGenders.FetchByNameAsync(updateDto.Gender);
            if (gender == null)
            {
                throw new GenderNameNotExistsException(updateDto.Gender);
            }
            AnimalLifeStatus? lifeStatus = await _context.AnimalLifeStatuses
                .FetchByNameAsync(updateDto.LifeStatus);
            if (lifeStatus == null)
            {
                throw new LifeStatusNameNotExistsException(updateDto.LifeStatus);
            }
            if (!await DoesChipperExistsAsync(updateDto.ChipperId))
            {
                throw new AccountNotFoundException(updateDto.ChipperId);
            }
            if (!await DoesChippingLocationExistsAsync(updateDto.ChippingLocationId))
            {
                throw new ChippingLocationNotFoundException(updateDto.ChippingLocationId);
            }
            Animal? fetchedAnimal = await _context.Animals.FetchByIdAsync(animalId);
            _logger.LogInformation("Fetched from database {@model}", fetchedAnimal);
            if (fetchedAnimal == null)
            {
                throw new AnimalNotFoundException(animalId);
            }
            if (IsDeadToAlive(fetchedAnimal, updateDto.LifeStatus))
            {
                throw new AnimalDeadToAliveException();
            }
            if (IsNewChippingPointSameAsFirstVisitedLocation(
                fetchedAnimal.VisitedLocations, updateDto.ChippingLocationId))
            {
                throw new NewChippingPointSameAsFirstVisitedLocationException(
                    updateDto.ChippingLocationId);
            }
            UpdateAnimal(fetchedAnimal, updateDto, gender, lifeStatus);
            await _context.SaveChangesAsync();
            return _mapper.Map<AnimalDto>(fetchedAnimal);
        }

        public async Task DeleteAsync(long? animalId)
        {
            Animal? fetchedAnimal = await _context.Animals.FetchByIdAsync(animalId);
            _logger.LogInformation("Fetched from database {@model}", fetchedAnimal);
            if (fetchedAnimal == null)
            {
                throw new AnimalNotFoundException(animalId);
            }
            if (await IsAnimalAssociatedWithVisitedLocationAsync(animalId))
            {
                throw new AnimalAssosiatedWithVisitedLocationException(animalId);
            }
            RemoveAnimal(fetchedAnimal);
            await _context.SaveChangesAsync();
        }

        public async Task<AnimalDto> AddTypeAsync(long? animalId, long? typeId)
        {
            Animal? fetchedAnimal = await _context.Animals.FetchByIdAsync(animalId);
            _logger.LogInformation("Fetched from database {@model}", fetchedAnimal);
            if (fetchedAnimal == null)
            {
                throw new AnimalNotFoundException(animalId);
            }
            AnimalType? fetchedAnimalType = await _context.AnimalTypes.FindAsync(typeId);
            _logger.LogInformation("Fetched from database {@model}", fetchedAnimalType);
            if (fetchedAnimalType == null)
            {
                throw new AnimalTypeNotFoundException(typeId);
            }
            fetchedAnimal.Types.Add(fetchedAnimalType);
            await _context.SaveChangesAsync();
            return _mapper.Map<AnimalDto>(fetchedAnimal);
        }

        public async Task<AnimalDto> UpdateTypeAsync(
            long? animalId, AnimalAnimalTypeUpdateDto updateDto)
        {
            Animal? fetchedAnimal = await _context.Animals.FetchByIdAsync(animalId);
            _logger.LogInformation("Fetched from database {@model}", fetchedAnimal);
            if (fetchedAnimal == null)
            {
                throw new AnimalNotFoundException(animalId);
            }
            AnimalType? oldAnimalType = await _context.AnimalTypes.FindAsync(updateDto.OldTypeId);
            _logger.LogInformation("Fetched from database {@model}", oldAnimalType);
            if (oldAnimalType == null)
            {
                throw new AnimalTypeNotFoundException(updateDto.OldTypeId);
            }
            AnimalType? newAnimalType = await _context.AnimalTypes.FindAsync(updateDto.NewTypeId);
            _logger.LogInformation("Fetched from database {@model}", newAnimalType);
            if (newAnimalType == null)
            {
                throw new AnimalTypeNotFoundException(updateDto.NewTypeId);
            }
            if (!fetchedAnimal.Types.Contains(oldAnimalType))
            {
                throw new AnimalNotHaveTypeException(animalId, updateDto.OldTypeId);
            }
            if (fetchedAnimal.Types.Contains(newAnimalType))
            {
                throw new AnimalHasTypeException(animalId, updateDto.OldTypeId);
            }
            fetchedAnimal.Types.Remove(oldAnimalType);
            fetchedAnimal.Types.Add(newAnimalType);
            await _context.SaveChangesAsync();
            return _mapper.Map<AnimalDto>(fetchedAnimal);
        }

        public async Task<AnimalDto> DeleteTypeAsync(long? animalId, long? typeId)
        {
            Animal? fetchedAnimal = await _context.Animals.FetchByIdAsync(animalId);
            _logger.LogInformation("Fetched from database {@model}", fetchedAnimal);
            if (fetchedAnimal == null)
            {
                throw new AnimalNotFoundException(animalId);
            }
            AnimalType? fetchedAnimalType = await _context.AnimalTypes.FindAsync(typeId);
            _logger.LogInformation("Fetched from database {@model}", fetchedAnimalType);
            if (fetchedAnimalType == null)
            {
                throw new AnimalTypeNotFoundException(typeId);
            }
            if (!fetchedAnimal.Types.Contains(fetchedAnimalType))
            {
                throw new AnimalNotHaveTypeException(animalId, typeId);
            }
            if (DoesAnimalHasOnlyOneSameType(fetchedAnimal, fetchedAnimalType))
            {
                throw new AnimalHasOnlyOneSameTypeException(animalId, typeId);
            }
            fetchedAnimal.Types.Remove(fetchedAnimalType);
            await _context.SaveChangesAsync();
            return _mapper.Map<AnimalDto>(fetchedAnimal);
        }

        async Task<Animal> AddAnimalAsync(AnimalCreateDto createDto, AnimalGender gender)
        {
            Animal newAnimal = _mapper.Map<Animal>(createDto);
            newAnimal.Gender = gender;
            newAnimal.LifeStatus = await _context.AnimalLifeStatuses
                .FetchByEnumAsync(LifeStatus.Alive);
            newAnimal.ChippingDateTime = DateTime.UtcNow;
            newAnimal.Types = await _context.AnimalTypes.FetchByIdsAsync(createDto.Types);
            await _context.Animals.AddAsync(newAnimal);
            return newAnimal;
        }

        void UpdateAnimal(Animal fetchedAnimal, AnimalUpdateDto updateDto,
            AnimalGender gender, AnimalLifeStatus lifeStatus)
        {
            _mapper.Map(updateDto, fetchedAnimal);
            if (IsAliveToDead(fetchedAnimal, lifeStatus.LifeStatus))
            {
                fetchedAnimal.DeathDateTime = DateTime.UtcNow;
            }
            fetchedAnimal.LifeStatus = lifeStatus;
            fetchedAnimal.Gender = gender;
        }

        void RemoveAnimal(Animal fetchedAnimal)
        {
            fetchedAnimal.Types.Clear();
            _context.Animals.Remove(fetchedAnimal);
        }

        async Task<bool> DoesChipperExistsAsync(int? chipperId)
        {
            return await _context.Accounts.DoesExistsAsync(chipperId);
        }

        async Task<bool> DoesChippingLocationExistsAsync(long? chippingLocationId)
        {
            return await _context.Locations.DoesExistsAsync(chippingLocationId);
        }

        async Task<bool> IsAnimalAssociatedWithVisitedLocationAsync(long? animalId)
        {
            return await _context.AnimalVisitedLocations.IsAnyAssociatedWithAnimal(animalId);
        }

        bool IsNewChippingPointSameAsFirstVisitedLocation(
            ICollection<AnimalVisitedLocation> visitedLocations, long? newChippingLocationId)
        {
            return visitedLocations.FirstOrDefault()?.LocationId == newChippingLocationId;
        }

        bool DoesAnimalHasOnlyOneSameType(Animal fetchedAnimal, AnimalType fetchedAnimalType)
        {
            return fetchedAnimal.Types.Count == 1
                && fetchedAnimal.Types.First().Id == fetchedAnimalType.Id;
        }

        bool IsDeadToAlive(Animal animal, string nextLifeStatus)
        {
            LifeStatus? currentLifeStatusEnum = ParseLifeStatus(animal.LifeStatus.LifeStatus);
            LifeStatus? nextLifeStatusEnum = ParseLifeStatus(nextLifeStatus);
            return currentLifeStatusEnum == LifeStatus.Dead
                && nextLifeStatusEnum == LifeStatus.Alive;
        }

        bool IsAliveToDead(Animal animal, string nextLifeStatus)
        {
            LifeStatus? currentLifeStatusEnum = ParseLifeStatus(animal.LifeStatus.LifeStatus);
            LifeStatus? nextLifeStatusEnum = ParseLifeStatus(nextLifeStatus);
            return currentLifeStatusEnum == LifeStatus.Alive
                && nextLifeStatusEnum == LifeStatus.Dead;
        }

        LifeStatus? ParseLifeStatus(string lifeStatus)
        {
            Enum.TryParse(typeof(LifeStatus), lifeStatus, true, out object? result);
            return (LifeStatus?)result;
        }
    }
}
