using AutoMapper;
using Core.Enumerations;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Data.Extensions;
using Microsoft.Extensions.Logging;
using Services.Dtos;
using Services.Search;
using System.Security.Claims;

namespace Services.Management
{
    public class AccountManagementService : IAccountManagementService
    {
        readonly ILogger<AccountManagementService> _logger;
        readonly ChippedAnimalsDbContext _context;
        readonly IAccountSearchService _accountSearchService;
        readonly IMapper _mapper;

        public AccountManagementService(
            ILogger<AccountManagementService> logger,
            ChippedAnimalsDbContext context,
            IAccountSearchService accountSearchService,
            IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _accountSearchService = accountSearchService;
            _mapper = mapper;
        }

        public async Task<AccountDto> GetByIdAsync(int? accountId, IEnumerable<Claim> claims)
        {
            Account? fetchedAccount = await _context.Accounts.FetchByIdNoTrackingAsync(accountId);
            _logger.LogInformation("Fetched from database {@model}", fetchedAccount);
            if (fetchedAccount == null)
            {
                if (IsAdmin(GetClaimRole(claims)))
                {
                    throw new AccountNotFoundException(accountId);
                }
                throw new AccountNotFoundForbiddenException(accountId);
            }
            return _mapper.Map<AccountDto>(fetchedAccount);
        }

        public async Task<IEnumerable<AccountDto>> GetListByFiltersAsync(AccountListDto listDto)
        {
            IEnumerable<Account> fetchedAccounts = await _accountSearchService
                .FindListByFiltersAsync(listDto);
            _logger.LogInformation("Fetched from database {@models}", fetchedAccounts);
            return _mapper.Map<IEnumerable<AccountDto>>(fetchedAccounts);
        }

        public async Task<AccountDto> CreateAsync(AccountCreateDto createDto)
        {
            AccountRole? accountRole = await _context.AccountRoles.FetchByNameAsync(createDto.Role);
            if (accountRole == null)
            {
                throw new RoleNameNotExistsException(createDto.Role);
            }
            if (await DoesEmailExistsAsync(createDto.Email))
            {
                throw new AccountEmailExistsException(createDto.Email);
            }
            Account newAccount = _mapper.Map<Account>(createDto);
            newAccount.Role = accountRole;
            await _context.Accounts.AddAsync(newAccount);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Added to database: {@model}", newAccount);
            return _mapper.Map<AccountDto>(newAccount);
        }

        public async Task<AccountDto> UpdateAsync(
            int? accountId, AccountUpdateDto updateDto, IEnumerable<Claim> claims)
        {
            AccountRole? accountRole = await _context.AccountRoles.FetchByNameAsync(updateDto.Role);
            if (accountRole == null)
            {
                throw new RoleNameNotExistsException(updateDto.Role);
            }
            Account? fetchedAccount = await _context.Accounts.FetchByIdAsync(accountId);
            _logger.LogInformation("Fetched from database {@model}", fetchedAccount);
            Role claimRole = GetClaimRole(claims);
            if (fetchedAccount == null)
            {
                if (IsAdmin(claimRole))
                {
                    throw new AccountNotFoundException(accountId);
                }
                throw new AccountNotFoundForbiddenException(accountId);
            }
            string claimEmail = GetClaimEmail(claims);
            if (!IsAdmin(claimRole)
                && !AreSameAddresses(fetchedAccount.Email, claimEmail))
            {
                throw new AccountNotFoundForbiddenException(accountId);
            }
            if (!AreSameAddresses(updateDto.Email, fetchedAccount.Email)
                && await DoesEmailExistsAsync(updateDto.Email))
            {
                throw new AccountEmailExistsException(updateDto.Email);
            }
            _mapper.Map(updateDto, fetchedAccount);
            fetchedAccount.Role = accountRole;
            await _context.SaveChangesAsync();
            return _mapper.Map<AccountDto>(fetchedAccount);
        }

        public async Task DeleteAsync(int? accountId, IEnumerable<Claim> claims)
        {
            Account? fetchedAccount = await _context.Accounts.FetchByIdAsync(accountId);
            _logger.LogInformation("Fetched from database {@model}", fetchedAccount);
            Role claimRole = GetClaimRole(claims);
            if (fetchedAccount == null)
            {
                if (IsAdmin(claimRole))
                {
                    throw new AccountNotFoundException(accountId);
                }
                throw new AccountNotFoundForbiddenException(accountId);
            }
            if (!IsAdmin(claimRole)
                && !AreSameAddresses(fetchedAccount.Email, GetClaimEmail(claims)))
            {
                throw new AccountNotFoundForbiddenException(accountId);
            }
            if (await IsAccountAssociatedWithAnimalAsync(accountId))
            {
                throw new AccountAssosiatedWithAnimalException(accountId);
            }
            _context.Accounts.Remove(fetchedAccount);
            await _context.SaveChangesAsync();
        }

        async Task<bool> IsAccountAssociatedWithAnimalAsync(int? accountId)
        {
            return await _context.Animals.IsAnyAssociatedWithAccountAsync(accountId);
        }

        async Task<bool> DoesEmailExistsAsync(string newEmail)
        {
            return await _context.Accounts.DoesEmailExistsAsync(newEmail);
        }

        bool AreSameAddresses(string firstEmail, string secondEmail)
        {
            return firstEmail.ToLower() == secondEmail.ToLower();
        }

        bool IsAdmin(Role userRole)
        {
            return userRole == Role.Admin;
        }

        string GetClaimEmail(IEnumerable<Claim> claims)
        {
            return claims
                .First(c => c.Type == ClaimTypes.Name)
                .Value;
        }

        Role GetClaimRole(IEnumerable<Claim> claims)
        {
            return (Role)Enum
                .Parse(typeof(Role), claims
                    .First(c => c.Type == ClaimTypes.Role)
                    .Value, true);
        }
    }
}
