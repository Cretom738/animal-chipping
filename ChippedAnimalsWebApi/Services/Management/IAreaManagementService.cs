using Services.Dtos;

namespace Services.Management
{
    public interface IAreaManagementService
    {
        Task<AreaDto> GetByIdAsync(long? areaId);
        Task<AreaDto> CreateAsync(AreaCreateDto createDto);
        Task<AreaDto> UpdateAsync(long? areaId, AreaUpdateDto updateDto);
        Task DeleteAsync(long? areaId);
    }
}
