using Services.Dtos;

namespace Services.Management
{
    public interface IAnimalManagementService
    {
        Task<AnimalDto> GetByIdAsync(long? animalId);
        Task<IEnumerable<AnimalDto>> GetListByFiltersAsync(AnimalListDto listDto);
        Task<AnimalDto> CreateAsync(AnimalCreateDto createDto);
        Task<AnimalDto> UpdateAsync(long? animalId, AnimalUpdateDto updateDto);
        Task DeleteAsync(long? animalId);
        Task<AnimalDto> AddTypeAsync(long? animalId, long? typeId);
        Task<AnimalDto> UpdateTypeAsync(long? accountId, AnimalAnimalTypeUpdateDto updateDto);
        Task<AnimalDto> DeleteTypeAsync(long? animalId, long? typeId);
    }
}
