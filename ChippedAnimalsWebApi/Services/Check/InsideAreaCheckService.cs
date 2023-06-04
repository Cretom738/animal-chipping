using Core.Models;
using Microsoft.Extensions.Logging;
using Services.Common;
using Services.Common.Intersection;

namespace Services.Check
{
    public class InsideAreaCheckService : IInsideAreaCheckService
    {
        readonly ILogger<InsideAreaCheckService> _logger;
        readonly IAreaToPolygonMapService _areaToPolygonMapService;

        public InsideAreaCheckService(
            ILogger<InsideAreaCheckService> logger,
            IAreaToPolygonMapService areaToPolygonMapService)
        {
            _logger = logger;
            _areaToPolygonMapService = areaToPolygonMapService;
        }

        public bool Check(Location location, Area area)
        {
            IList<Point> points = MapAreaPointsToPoints(area);
            LogPoints(area, points);
            IList<Segment> segments = ConnectPointsIntoSegments(area.Name, points);
            LogSegments(segments);
            Segment locationRay = GetLocationRay(location);
            LogRay(locationRay);
            int intersectionAmount = 0;
            foreach (Segment segment in segments)
            {
                Point? point = locationRay.GetIntersectionPoint(segment);
                if (point != null)
                {
                    if (point.Equals(locationRay.FirstPoint))
                    {
                        LogIntersection(point, locationRay, segment);
                        return true;
                    }
                    if (point.Y == Math.Max(segment.FirstPoint.Y, segment.SecondPoint.Y))
                    {
                        continue;
                    }
                    LogIntersection(point, locationRay, segment);
                    intersectionAmount++;
                }
            }
            return intersectionAmount % 2 == 1;
        }

        public bool Check(Area innerArea, Area outerArea)
        {
            IList<Point> points = MapAreaPointsToPoints(outerArea);
            LogPoints(outerArea, points);
            IList<Segment> segments = ConnectPointsIntoSegments(outerArea.Name, points);
            LogSegments(segments);
            Segment ray = new Segment(innerArea.Name, null!, new Point(181D, 0));
            foreach (AreaPoint areaPoint in innerArea.AreaPoints)
            {
                ray.FirstPoint = new Point(areaPoint.Longitude, areaPoint.Latitude);
                ray.SecondPoint.Y = areaPoint.Latitude;
                LogRay(ray);
                int intersectionAmount = 0;
                int sameAsStartPointIntersectionAmount = 0;
                foreach (Segment segment in segments)
                {
                    Point? point = ray.GetIntersectionPoint(segment);
                    if (point != null)
                    {
                        if (point.Y == Math.Max(segment.FirstPoint.Y, segment.SecondPoint.Y))
                        {
                            continue;
                        }
                        LogIntersection(point, ray, segment);
                        if (point.Equals(ray.FirstPoint))
                        {
                            sameAsStartPointIntersectionAmount++;
                            continue;
                        }
                        intersectionAmount++;
                    }
                }
                if (intersectionAmount > 0
                    && (intersectionAmount + sameAsStartPointIntersectionAmount) % 2 == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Check(Area area, IList<Area> areas)
        {
            foreach (Area currentArea in areas)
            {
                if (Check(area, currentArea))
                {
                    return true;
                }
            }
            return false;
        }

        IList<Point> MapAreaPointsToPoints(Area area)
        {
            return _areaToPolygonMapService.MapAreaPointsToPoints(area);
        }

        IList<Segment> ConnectPointsIntoSegments(string areaName, IList<Point> points)
        {
            return _areaToPolygonMapService.ConnectPointsIntoSegments(areaName, points);
        }

        Segment GetLocationRay(Location location)
        {
            return new Segment(string.Empty,
                new Point(location.Longitude, location.Latitude),
                new Point(181D, location.Latitude));
        }

        void LogPoints(Area area, IList<Point> points)
        {
            _logger.LogDebug("areaName: {name}, points:\n\t{points}",
                area.Name, string.Join(",\n\t", points));
        }

        void LogSegments(IList<Segment> segments)
        {
            _logger.LogDebug("segments:\n\t{segments}", string.Join(",\n\t", segments));
        }

        void LogRay(Segment ray)
        {
            _logger.LogDebug("Ray: {segment}", ray);
        }

        void LogIntersection(Point point, Segment ray, Segment segment)
        {
            _logger.LogDebug("Intersection on {point} {ray} {segment}", point, ray, segment);
        }
    }
}
