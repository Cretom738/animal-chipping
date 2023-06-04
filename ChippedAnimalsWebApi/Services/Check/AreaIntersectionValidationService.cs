using Core.Models;
using Microsoft.Extensions.Logging;
using Services.Common.Intersection;
using Services.Common;

namespace Services.Check
{
    public class AreaIntersectionValidationService : IAreaIntersectionValidationService
    {
        readonly ILogger<AreaIntersectionValidationService> _logger;
        readonly IIntersectionChecker _checker;
        readonly IAreaToPolygonMapService _areaToPolygonMapService;

        public AreaIntersectionValidationService(
            ILogger<AreaIntersectionValidationService> logger,
            IIntersectionChecker checker,
            IAreaToPolygonMapService areaToPolygonMapService)
        {
            _logger = logger;
            _checker = checker;
            _areaToPolygonMapService = areaToPolygonMapService;
        }

        public void Validate(Area newArea, IList<Area> allAreas)
        {
            foreach (Area currentArea in allAreas)
            {
                IList<Point> newAreaPoints = MapAreaPointsToPoints(newArea);
                IList<Point> currentAreaPoints = MapAreaPointsToPoints(currentArea);
                LogPoints(newArea, newAreaPoints);
                LogPoints(currentArea, currentAreaPoints);
                IList<Segment> newAreaSegments =
                    ConnectPointsIntoSegments(newArea.Name, newAreaPoints);
                IList<Segment> currentAreaSegments =
                    ConnectPointsIntoSegments(currentArea.Name, currentAreaPoints);
                IList<ValidIntersection> newAreaValidIntersections =
                    GetPolygonValidIntersections(newAreaSegments);
                IList<ValidIntersection> currentAreaValidIntersections =
                    GetPolygonValidIntersections(currentAreaSegments);
                IList<Segment> allSegments = newAreaSegments
                    .Union(currentAreaSegments)
                    .ToList();
                LogSegments(allSegments);
                IList<ValidIntersection> allValidIntersections = newAreaValidIntersections
                    .Union(currentAreaValidIntersections)
                    .ToList();
                MergeSameValidIntersectionPoints(allValidIntersections);
                LogValidIntersections(allValidIntersections);
                try
                {
                    _checker.Check(allSegments, allValidIntersections);
                    SwapPointCoordinates(newAreaPoints.Union(currentAreaPoints));
                    _checker.Check(allSegments, allValidIntersections, true);
                }
                catch (Exception)
                {
                    _checker.Clear();
                    throw;
                }
            }
        }

        IList<Point> MapAreaPointsToPoints(Area area)
        {
            return _areaToPolygonMapService.MapAreaPointsToPoints(area);
        }

        IList<Segment> ConnectPointsIntoSegments(string areaName, IList<Point> points)
        {
            return _areaToPolygonMapService.ConnectPointsIntoSegments(areaName, points);
        }

        IList<ValidIntersection> GetPolygonValidIntersections(IList<Segment> areaSegments)
        {
            return _areaToPolygonMapService.GetPolygonValidIntersections(areaSegments);
        }

        void MergeSameValidIntersectionPoints(IList<ValidIntersection> validIntersections)
        {
            _areaToPolygonMapService.MergeSameValidIntersectionPoints(validIntersections);
        }

        void SwapPointCoordinates(IEnumerable<Point> areaPoints)
        {
            _areaToPolygonMapService.SwapPointCoordinates(areaPoints);
        }

        void LogPoints(Area newArea, IList<Point> newAreaPoints)
        {
            _logger.LogDebug("areaName: {name}, points:\n\t{points}",
                newArea.Name, string.Join(",\n\t", newAreaPoints));
        }

        void LogSegments(IList<Segment> allSegments)
        {
            _logger.LogDebug("segments:\n\t{segments}", string.Join(",\n\t", allSegments));
        }

        void LogValidIntersections(IList<ValidIntersection> allValidIntersections)
        {
            _logger.LogDebug("validIntersections:\n{validIntersections}",
                string.Join(",\n", allValidIntersections));
        }
    }
}
