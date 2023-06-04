using Services.Dtos;
using System.Security.Claims;

namespace Services.Management
{
    public interface IAccountManagementService
    {
        Task<AccountDto> GetByIdAsync(int? accountId, IEnumerable<Claim> claims);
        Task<IEnumerable<AccountDto>> GetListByFiltersAsync(AccountListDto listDto);
        Task<AccountDto> CreateAsync(AccountCreateDto createDto);
        Task<AccountDto> UpdateAsync(int? accountId, AccountUpdateDto updateDto, IEnumerable<Claim> claims);
        Task DeleteAsync(int? accountId, IEnumerable<Claim> claims);
    }
}
