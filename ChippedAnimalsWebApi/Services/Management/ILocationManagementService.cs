using Services.Dtos;

namespace Services.Management
{
    public interface ILocationManagementService
    {
        Task<LocationDto> GetByIdAsync(long? pointId);
        Task<long> GetIdByCoordinatesAsync(LocationShowDto showDto);
        Task<LocationDto> CreateAsync(LocationCreateDto createDto);
        Task<LocationDto> UpdateAsync(long? pointId, LocationUpdateDto updateDto);
        Task DeleteAsync(long? pointId);
    }
}
