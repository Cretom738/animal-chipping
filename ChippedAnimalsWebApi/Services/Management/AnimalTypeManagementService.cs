using AutoMapper;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Data.Extensions;
using Microsoft.Extensions.Logging;
using Services.Dtos;

namespace Services.Management
{
    public class AnimalTypeManagementService : IAnimalTypeManagementService
    {
        readonly ILogger<AnimalTypeManagementService> _logger;
        readonly ChippedAnimalsDbContext _context;
        readonly IMapper _mapper;

        public AnimalTypeManagementService(
            ILogger<AnimalTypeManagementService> logger,
            ChippedAnimalsDbContext context,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<AnimalTypeDto> GetByIdAsync(long? typeId)
        {
            AnimalType? fetchedAnimalType = await _context.AnimalTypes
                .FetchByIdNoTrackingAsync(typeId);
            _logger.LogInformation("Fetched from database {@model}", fetchedAnimalType);
            if (fetchedAnimalType == null)
            {
                throw new AnimalTypeNotFoundException(typeId);
            }
            return _mapper.Map<AnimalTypeDto>(fetchedAnimalType);
        }

        public async Task<AnimalTypeDto> CreateAsync(AnimalTypeCreateDto createDto)
        {
            if (await DoesAnimalTypeNameExists(createDto.Type))
            {
                throw new AnimalTypeNameExistsException(createDto.Type);
            }
            AnimalType newAnimalType = _mapper.Map<AnimalType>(createDto);
            await _context.AnimalTypes.AddAsync(newAnimalType);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Added to database: {@model}", newAnimalType);
            return _mapper.Map<AnimalTypeDto>(newAnimalType);
        }

        public async Task<AnimalTypeDto> UpdateAsync(long? typeId, AnimalTypeUpdateDto updateDto)
        {
            if (await DoesAnimalTypeNameExists(updateDto.Type))
            {
                throw new AnimalTypeNameExistsException(updateDto.Type);
            }
            AnimalType? fetchedAnimalType = await _context.AnimalTypes.FindAsync(typeId);
            _logger.LogInformation("Fetched from database {@model}", fetchedAnimalType);
            if (fetchedAnimalType == null)
            {
                throw new AnimalTypeNotFoundException(typeId);
            }
            _mapper.Map(updateDto, fetchedAnimalType);
            await _context.SaveChangesAsync();
            return _mapper.Map<AnimalTypeDto>(fetchedAnimalType);
        }

        public async Task DeleteAsync(long? typeId)
        {
            AnimalType? fetchedAnimalType = await _context.AnimalTypes.FindAsync(typeId);
            _logger.LogInformation("Fetched from database {@model}", fetchedAnimalType);
            if (fetchedAnimalType == null)
            {
                throw new AnimalTypeNotFoundException(typeId);
            }
            if (await IsAnimalTypeAssociatedWithAnimal(typeId))
            {
                throw new AnimalTypeAssociatedWithAnimalException(typeId);
            }
            _context.AnimalTypes.Remove(fetchedAnimalType);
            await _context.SaveChangesAsync();
        }

        async Task<bool> DoesAnimalTypeNameExists(string typeName)
        {
            return await _context.AnimalTypes.DoesNameExistsAsync(typeName);
        }

        async Task<bool> IsAnimalTypeAssociatedWithAnimal(long? typeId)
        {
            return await _context.Animals.IsAnyAssociatedWithAnimalTypeAsync(typeId);
        }
    }
}
