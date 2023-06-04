using Core.Models;
using Services.Dtos;

namespace Services.Search
{
    public interface IAnimalVisitedLocationSearchService
    {
        Task<IEnumerable<AnimalVisitedLocation>> FindListByFiltersAsync(
            long? animalId, AnimalVisitedLocationListDto filterParameters);
    }
}
