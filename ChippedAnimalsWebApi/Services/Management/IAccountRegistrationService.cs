using Services.Dtos;

namespace Services.Management
{
    public interface IAccountRegistrationService
    {
        Task<AccountDto> CreateAsync(AccountRegistrationDto registrationDto);
    }
}
