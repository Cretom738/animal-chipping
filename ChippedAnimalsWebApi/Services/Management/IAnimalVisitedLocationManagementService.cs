using Services.Dtos;

namespace Services.Management
{
    public interface IAnimalVisitedLocationManagementService
    {
        Task<IEnumerable<AnimalVisitedLocationDto>> GetListByFiltersAsync(
            long? animalId, AnimalVisitedLocationListDto listDto);
        Task<AnimalVisitedLocationDto> CreateAsync(long? animalId, long? pointId);
        Task<AnimalVisitedLocationDto> UpdateAsync(
            long? animalId, AnimalVisitedLocationUpdateDto updateDto);
        Task DeleteAsync(long? animalId, long? visitedPointId);
    }
}
