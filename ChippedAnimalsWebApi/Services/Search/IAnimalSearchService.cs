using Core.Models;
using Services.Dtos;

namespace Services.Search
{
    public interface IAnimalSearchService
    {
        Task<IEnumerable<Animal>> FindListByFiltersAsync(AnimalListDto filterParameters);
    }
}
