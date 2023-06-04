using Services.Dtos;

namespace Services.Management
{
    public interface IAnimalTypeManagementService
    {
        Task<AnimalTypeDto> GetByIdAsync(long? typeId);
        Task<AnimalTypeDto> CreateAsync(AnimalTypeCreateDto createDto);
        Task<AnimalTypeDto> UpdateAsync(long? typeId, AnimalTypeUpdateDto updateDto);
        Task DeleteAsync(long? typeId);
    }
}
