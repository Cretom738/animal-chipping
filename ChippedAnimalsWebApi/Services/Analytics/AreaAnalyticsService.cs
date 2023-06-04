using Services.Dtos;
using Core.Exceptions;
using Core.Models;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Infrastructure.Data.Extensions;
using Services.Check;
using System.Globalization;

namespace Services.Analytics
{
    public class AreaAnalyticsService : IAreaAnalyticsService
    {
        readonly ILogger<AreaAnalyticsService> _logger;
        readonly ChippedAnimalsDbContext _context;
        readonly IInsideAreaCheckService _locationInsideAreaCheckService;

        public AreaAnalyticsService(
            ILogger<AreaAnalyticsService> logger,
            ChippedAnimalsDbContext context,
            IInsideAreaCheckService locationInsideAreaCheckService)
        {
            _logger = logger;
            _context = context;
            _locationInsideAreaCheckService = locationInsideAreaCheckService;
        }

        public async Task<AreaAnalyticsDto> GetAreaAnalyticsAsync(
            long? areaId, AreaAnalyticsShowDto interval)
        {
            DateTime startDate = ParseDate(interval.StartDate);
            DateTime endDate = ParseDate(interval.EndDate);
            if (startDate >= endDate)
            {
                throw new AnalyticsStartDateLaterOrEqualToEndDateException(
                    startDate, endDate);
            }
            Area? fetchedArea = await _context.Areas.FetchByIdNoTrackingAsync(areaId);
            _logger.LogInformation("Fetched from database {@model}", fetchedArea);
            if (fetchedArea == null)
            {
                throw new AreaNotFoundException(areaId);
            }
            return await GetAreaAnalytics(fetchedArea, startDate, endDate);
        }

        async Task<AreaAnalyticsDto> GetAreaAnalytics(
            Area area, DateTime startDate, DateTime endDate)
        {
            IList<AnimalAnalyticsDto> animalAnalyticsList = new List<AnimalAnalyticsDto>();
            AreaAnalyticsDto areaAnalyticsDto = new AreaAnalyticsDto
            {
                AnimalAnalytics = animalAnalyticsList
            };
            IList<AnimalType> allAnimalTypes = await _context.AnimalTypes
                .FetchListForAnalyticsAsync();
            _logger.LogInformation("Fetched types: {@models}", allAnimalTypes);
            IList<Animal> analysedAnimals = new List<Animal>();
            foreach (AnimalType animalType in allAnimalTypes)
            {
                AnimalAnalyticsDto animalAnalytics = new AnimalAnalyticsDto
                {
                    AnimalType = animalType.Type,
                    AnimalTypeId = animalType.Id
                };
                foreach (Animal animal in animalType.Animals)
                {
                    IList<AnimalVisitedLocation> visitedDuringInterval = animal
                        .VisitedLocations
                        .Where(avl => IsDateInInterval(avl.VisitDateTime, startDate, endDate))
                        .OrderBy(avl => avl.VisitDateTime)
                        .ToList();
                    _logger.LogInformation("Animal: {@animal} visited during interval locations: {@models}", 
                        animal, visitedDuringInterval);
                    bool hasArrived = false;
                    bool isInArea = false;
                    bool hasGone = false;
                    if (IsDateInInterval(animal.ChippingDateTime, startDate, endDate)
                        && _locationInsideAreaCheckService.Check(animal.ChippingLocation, area))
                    {
                        isInArea = true;
                    }
                    for (int i = 0; i < visitedDuringInterval.Count; i++)
                    {
                        if (_locationInsideAreaCheckService.Check(
                            visitedDuringInterval[i].Location, area))
                        {
                            if (!isInArea)
                            {
                                hasArrived = true;
                                isInArea = true;
                            }
                        }
                        else
                        {
                            if (isInArea)
                            {
                                hasGone = true;
                                isInArea = false;
                            }
                        }
                    }
                    _logger.LogTrace("hasArrived: {hasArrived}", hasArrived);
                    _logger.LogTrace("isInArea: {isInArea}", isInArea);
                    _logger.LogTrace("hasGone: {hasGone}", hasGone);
                    animalAnalytics.ArrivedAnimalQuantity += hasArrived ? 1 : 0;
                    animalAnalytics.AnimalQuantity += isInArea ? 1 : 0;
                    animalAnalytics.GoneAnimalQuantity += hasGone ? 1 : 0;
                    if (!analysedAnimals.Contains(animal))
                    {
                        analysedAnimals.Add(animal);
                        areaAnalyticsDto.TotalArrivedAnimalQuantity += hasArrived ? 1 : 0;
                        areaAnalyticsDto.TotalAnimalQuantity += isInArea ? 1 : 0;
                        areaAnalyticsDto.TotalGoneAnimalQuantity += hasGone ? 1 : 0;
                    }
                }
                _logger.LogInformation("animalAnalytics: {analytics}", animalAnalytics);
                if (!AreAnimalAnalyticsEmpty(animalAnalytics))
                {
                    animalAnalyticsList.Add(animalAnalytics);
                }
            }
            return areaAnalyticsDto;
        }

        DateTime ParseDate(string date)
        {
            return DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        bool IsDateInInterval(DateTime date, DateTime startDate, DateTime endDate)
        {
            return date >= startDate && date <= endDate;
        }

        bool AreAnimalAnalyticsEmpty(AnimalAnalyticsDto animalAnalytics)
        {
            return animalAnalytics.ArrivedAnimalQuantity == 0
                && animalAnalytics.AnimalQuantity == 0
                && animalAnalytics.GoneAnimalQuantity == 0;
        }
    }
}
