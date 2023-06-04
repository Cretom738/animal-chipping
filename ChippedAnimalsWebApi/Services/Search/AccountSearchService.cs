using Core.Models;
using Infrastructure.Data;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Dtos;

namespace Services.Search
{
    public class AccountSearchService : IAccountSearchService
    {
        readonly ILogger<AccountSearchService> _logger;
        readonly ChippedAnimalsDbContext _context;

        public AccountSearchService(
            ILogger<AccountSearchService> logger,
            ChippedAnimalsDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<Account>> FindListByFiltersAsync(
            AccountListDto filterParameters)
        {
            IQueryable<Account> query = _context.Accounts;
            query = FilterByFirstName(query, filterParameters.FirstName);
            query = FilterByLastName(query, filterParameters.LastName);
            query = FilterByEmail(query, filterParameters.Email);
            query = ApplyPaging(query, filterParameters.From, filterParameters.Size);
            return await query.AsNoTracking().FetchListAsync();
        }

        IQueryable<Account> FilterByFirstName(IQueryable<Account> query, string? firstName)
        {
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query = query.WhereFirstName(firstName);
                _logger.LogTrace("Filtered by firstName: {parameter}", firstName);
            }
            return query;
        }

        IQueryable<Account> FilterByLastName(IQueryable<Account> query, string? lastName)
        {
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                query = query.WhereLastName(lastName);
                _logger.LogTrace("Filtered by lastName: {parameter}", lastName);
            }
            return query;
        }

        IQueryable<Account> FilterByEmail(IQueryable<Account> query, string? email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.WhereEmail(email);
                _logger.LogTrace("Filtered by email: {parameter}", email);
            }
            return query;
        }

        IQueryable<Account> ApplyPaging(IQueryable<Account> query, int? offset, int? amount)
        {
            query = query.OrderById();
            _logger.LogTrace("Ordered by id");
            query = query.Skip(offset ?? 0);
            _logger.LogTrace("Skipped: {parameter}", offset);
            query = query.Take(amount ?? 10);
            _logger.LogTrace("Taken: {parameter}", amount);
            return query;
        }
    }
}
