using Core.Models;
using Infrastructure.Data;
using Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Services.Dtos;

namespace Services.Search
{
    public class AnimalSearchService : IAnimalSearchService
    {
        readonly ILogger<AnimalSearchService> _logger;
        readonly ChippedAnimalsDbContext _context;

        public AnimalSearchService(
            ILogger<AnimalSearchService> logger,
            ChippedAnimalsDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IEnumerable<Animal>> FindListByFiltersAsync(
            AnimalListDto filterParameters)
        {
            IQueryable<Animal> query = _context.Animals;
            query = FilterByStartChippingDateTime(query, filterParameters.StartDateTime);
            query = FilterByEndChippingDateTime(query, filterParameters.EndDateTime);
            query = FilterByChipperId(query, filterParameters.ChipperId);
            query = FilterByChippingLocationId(query, filterParameters.ChippingLocationId);
            query = await FilterByLifeStatus(query, filterParameters.LifeStatus);
            query = await FilterByGender(query, filterParameters.Gender);
            query = ApplyPaging(query, filterParameters.From, filterParameters.Size);
            return await query.AsNoTracking().FetchListAsync();
        }

        IQueryable<Animal> FilterByStartChippingDateTime(IQueryable<Animal> query,
            DateTime? startChippingDateTime)
        {
            if (startChippingDateTime != null)
            {
                query = query.WhereStartDateTime(startChippingDateTime);
                _logger.LogTrace("Filtered by startDateTime: {parameter}", startChippingDateTime);
            }
            return query;
        }

        IQueryable<Animal> FilterByEndChippingDateTime(IQueryable<Animal> query,
            DateTime? endChippingDateTime)
        {
            if (endChippingDateTime != null)
            {
                query = query.WhereEndDateTime(endChippingDateTime);
                _logger.LogTrace("Filtered by endDateTime: {parameter}", endChippingDateTime);
            }
            return query;
        }

        IQueryable<Animal> FilterByChipperId(IQueryable<Animal> query, int? chipperId)
        {
            if (chipperId != null)
            {
                query = query.WhereChipperId(chipperId);
                _logger.LogTrace("Filtered by chipperId: {parameter}", chipperId);
            }
            return query;
        }

        IQueryable<Animal> FilterByChippingLocationId(
            IQueryable<Animal> query, long? chippingLocationId)
        {
            if (chippingLocationId != null)
            {
                query = query.WhereChippingLocationId(chippingLocationId);
                _logger.LogTrace("Filtered by chippingLocationId: {parameter}",
                    chippingLocationId);
            }
            return query;
        }

        async Task<IQueryable<Animal>> FilterByLifeStatus(
            IQueryable<Animal> query, string? lifeStatus)
        {
            if (await DoesLifeStatusExists(lifeStatus))
            {
                query = query.WhereLifeStatus(lifeStatus!);
                _logger.LogTrace("Filtered by lifeStatus: {parameter}", lifeStatus!.ToUpper());
            }
            return query;
        }

        async Task<IQueryable<Animal>> FilterByGender(IQueryable<Animal> query, string? gender)
        {
            if (await DoesGenderExists(gender))
            {
                query = query.WhereGender(gender!);
                _logger.LogTrace("Filtered by gender: {parameter}", gender!.ToUpper());
            }
            return query;
        }

        async Task<bool> DoesLifeStatusExists(string? lifeStatus)
        {
            if (!string.IsNullOrWhiteSpace(lifeStatus))
            {
                return await _context.AnimalLifeStatuses.DoesNameExistsAsync(lifeStatus);
            }
            return false;
        }

        async Task<bool> DoesGenderExists(string? gender)
        {
            if (!string.IsNullOrWhiteSpace(gender))
            {
                return await _context.AnimalGenders.DoesNameExistsAsync(gender);
            }
            return false;
        }

        IQueryable<Animal> ApplyPaging(IQueryable<Animal> query, int? offset, int? amount)
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
