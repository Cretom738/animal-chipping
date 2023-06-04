using Microsoft.Extensions.DependencyInjection;
using Services.Analytics;
using Services.Check;
using Services.Common;
using Services.Management;
using Services.Processing;
using Services.Search;

namespace Services.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddManagementServices();
            services.AddSearchServices();
            services.AddCheckServices();
            services.AddAnalyticsServices();
            services.AddCommonServices();
            services.AddProcessingServices();
        }

        public static void AddManagementServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountRegistrationService, AccountRegistrationService>();
            services.AddScoped<IAccountManagementService, AccountManagementService>();
            services.AddScoped<IAnimalManagementService, AnimalManagementService>();
            services.AddScoped<IAnimalTypeManagementService, AnimalTypeManagementService>();
            services.AddScoped<IAnimalVisitedLocationManagementService, AnimalVisitedLocationManagementService>();
            services.AddScoped<IAreaManagementService, AreaManagementService>();
            services.AddScoped<ILocationManagementService, LocationManagementService>();
        }

        public static void AddSearchServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountSearchService, AccountSearchService>();
            services.AddScoped<IAnimalSearchService, AnimalSearchService>();
            services.AddScoped<IAnimalVisitedLocationSearchService, AnimalVisitedLocationSearchService>();
        }

        public static void AddCheckServices(this IServiceCollection services)
        {
            services.AddScoped<IAreaIntersectionValidationService, AreaIntersectionValidationService>();
            services.AddScoped<IAreaPointsCoincidenceValidationService, AreaPointsCoincidenceValidationService>();
            services.AddScoped<IIntersectionChecker, IntersectionChecker>();
            services.AddScoped<IInsideAreaCheckService, InsideAreaCheckService>();
        }

        public static void AddAnalyticsServices(this IServiceCollection services)
        {
            services.AddScoped<IAreaAnalyticsService, AreaAnalyticsService>();
        }

        public static void AddCommonServices(this IServiceCollection services)
        {
            services.AddScoped<IAreaToPolygonMapService, AreaToPolygonMapService>();
        }

        public static void AddProcessingServices(this IServiceCollection services)
        {
            services.AddScoped<IGeohashService, GeohashService>();
        }
    }
}
