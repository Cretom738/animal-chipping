using AutoMapper;
using Core.Enumerations;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Data.Extensions;
using Microsoft.Extensions.Logging;
using Services.Dtos;

namespace Services.Management
{
    public class AccountRegistrationService : IAccountRegistrationService
    {
        readonly ILogger<AccountRegistrationService> _logger;
        readonly ChippedAnimalsDbContext _context;
        readonly IMapper _mapper;

        public AccountRegistrationService(
            ILogger<AccountRegistrationService> logger,
            ChippedAnimalsDbContext context,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<AccountDto> CreateAsync(AccountRegistrationDto registrationDto)
        {
            if (await DoesEmailExistsAsync(registrationDto.Email))
            {
                throw new AccountEmailExistsException(registrationDto.Email);
            }
            Account newAccount = _mapper.Map<Account>(registrationDto);
            newAccount.Role = await _context.AccountRoles.FetchByEnumAsync(Role.User);
            await _context.Accounts.AddAsync(newAccount);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Added to database: {@model}", newAccount);
            return _mapper.Map<AccountDto>(newAccount);
        }

        async Task<bool> DoesEmailExistsAsync(string newEmail)
        {
            return await _context.Accounts.DoesEmailExistsAsync(newEmail);
        }
    }
}
