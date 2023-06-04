using Core.Models;
using Services.Dtos;

namespace Services.Search
{
    public interface IAccountSearchService
    {
        Task<IEnumerable<Account>> FindListByFiltersAsync(AccountListDto filterParameters);
    }
}
