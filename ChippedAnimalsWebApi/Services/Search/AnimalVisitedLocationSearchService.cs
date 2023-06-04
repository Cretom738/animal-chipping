using Core.Exceptions;
using Core.Models;
using Infrastructure.Data;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Dtos;

namespace Services.Search
{
    public class AnimalVisitedLocationSearchService : IAnimalVisitedLocationSearchService
    {
        readonly ILogger<AnimalVisitedLocationSearchService> _logger;
        readonly ChippedAnimalsDbContext _context;

        public AnimalVisitedLocationSearchService(
            ILogger<AnimalVisitedLocationSearchService> logger,
            ChippedAnimalsDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<AnimalVisitedLocation>> FindListByFiltersAsync(
            long? animalId, AnimalVisitedLocationListDto filterParameters)
        {
            if (!await DoesAnimalExistsAsync(animalId))
            {
                throw new AnimalNotFoundException(animalId);
            }
            IQueryable<AnimalVisitedLocation> query = GetVisitedLocationsWithAimalId(animalId);
            query = FilterByStartVisitDateTime(query, filterParameters.StartDateTime);
            query = FilterByEndVisitDateTime(query, filterParameters.EndDateTime);
            query = ApplyPaging(query, filterParameters.From, filterParameters.Size);
            return await query.AsNoTracking().ToListAsync();
        }

        IQueryable<AnimalVisitedLocation> GetVisitedLocationsWithAimalId(long? animalId)
        {
            return _context.AnimalVisitedLocations.WhereAnimalId(animalId);
        }

        IQueryable<AnimalVisitedLocation> FilterByStartVisitDateTime(
            IQueryable<AnimalVisitedLocation> query, DateTime? startVisitDateTime)
        {
            if (startVisitDateTime != null)
            {
                query = query.WhereStartDateTime(startVisitDateTime);
                _logger.LogTrace("Filtered by startDateTime: {parameter}", startVisitDateTime);
            }
            return query;
        }

        IQueryable<AnimalVisitedLocation> FilterByEndVisitDateTime(
            IQueryable<AnimalVisitedLocation> query, DateTime? endVisitDateTime)
        {
            if (endVisitDateTime != null)
            {
                query = query.WhereEndDateTime(endVisitDateTime);
                _logger.LogTrace("Filtered by endDateTime: {parameter}", endVisitDateTime);
            }
            return query;
        }

        IQueryable<AnimalVisitedLocation> ApplyPaging(
            IQueryable<AnimalVisitedLocation> query, int? offset, int? amount)
        {
            query = query.OrderByVisitDateTime();
            _logger.LogTrace("Ordered by visitDateTime");
            query = query.Skip(offset ?? 0);
            _logger.LogTrace("Skipped: {parameter}", offset);
            query = query.Take(amount ?? 10);
            _logger.LogTrace("Taken: {parameter}", amount);
            return query;
        }

        async Task<bool> DoesAnimalExistsAsync(long? animalId)
        {
            return await _context.Animals.DoesExistsAsync(animalId);
        }
    }
}
