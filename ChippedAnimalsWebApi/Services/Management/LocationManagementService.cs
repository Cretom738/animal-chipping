using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Data.Extensions;
using Microsoft.Extensions.Logging;
using Services.Dtos;

namespace Services.Management
{
    public class LocationManagementService : ILocationManagementService
    {
        readonly ILogger<LocationManagementService> _logger;
        readonly ChippedAnimalsDbContext _context;
        readonly IMapper _mapper;

        public LocationManagementService(
            ILogger<LocationManagementService> logger,
            ChippedAnimalsDbContext context,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<LocationDto> GetByIdAsync(long? pointId)
        {
            Location? fetchedLocation = await _context.Locations.FetchByIdNoTrackingAsync(pointId);
            _logger.LogInformation("Fetched from database {@model}", fetchedLocation);
            if (fetchedLocation == null)
            {
                throw new LocationNotFoundException(pointId);
            }
            return _mapper.Map<LocationDto>(fetchedLocation);
        }

        public async Task<long> GetIdByCoordinatesAsync(LocationShowDto showDto)
        {
            long pointId = await _context.Locations
                .FetchIdByCoordinatesAsync(showDto.Latitude, showDto.Longitude);
            _logger.LogInformation("Fetched id {id}", pointId);
            if (pointId == 0)
            {
                throw new LocationNotFoundException(showDto.Latitude, showDto.Longitude);
            }
            return pointId;
        }

        public async Task<LocationDto> CreateAsync(LocationCreateDto createDto)
        {
            if (await DoCoordinatesExistAsync(createDto.Latitude, createDto.Longitude))
            {
                throw new LocationCoordinatesExistsException(
                    createDto.Latitude, createDto.Longitude);
            }
            Location newLocation = _mapper.Map<Location>(createDto);
            await _context.Locations.AddAsync(newLocation);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Added to database: {@model}", newLocation);
            return _mapper.Map<LocationDto>(newLocation);
        }

        public async Task<LocationDto> UpdateAsync(long? pointId, LocationUpdateDto updateDto)
        {
            if (await DoCoordinatesExistAsync(updateDto.Latitude, updateDto.Longitude))
            {
                throw new LocationCoordinatesExistsException(
                    updateDto.Latitude, updateDto.Longitude);
            }
            Location? fetchedLocation = await _context.Locations.FindAsync(pointId);
            _logger.LogInformation("Fetched from database {@model}", fetchedLocation);
            if (fetchedLocation == null)
            {
                throw new LocationNotFoundException(pointId);
            }
            _mapper.Map(updateDto, fetchedLocation);
            await _context.SaveChangesAsync();
            return _mapper.Map<LocationDto>(fetchedLocation);
        }

        public async Task DeleteAsync(long? pointId)
        {
            Location? fetchedLocation = await _context.Locations.FindAsync(pointId);
            _logger.LogInformation("Fetched from database {@model}", fetchedLocation);
            if (fetchedLocation == null)
            {
                throw new LocationNotFoundException(pointId);
            }
            if (await IsLocationAssociatedWithAnimalAsync(pointId))
            {
                throw new LocationAssociatedWithAnimalException(pointId);
            }
            _context.Locations.Remove(fetchedLocation);
            await _context.SaveChangesAsync();
        }

        async Task<bool> DoCoordinatesExistAsync(double? latitude, double? longitude)
        {
            return await _context.Locations.DoCoordinatesExistAsync(latitude, longitude);
        }

        async Task<bool> IsLocationAssociatedWithAnimalAsync(long? pointId)
        {
            return await _context.Animals.IsAnyAssociatedWithLocationAsync(pointId);
        }
    }
}
