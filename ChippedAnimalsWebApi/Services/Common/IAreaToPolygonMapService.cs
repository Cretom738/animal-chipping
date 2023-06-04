using Core.Models;
using Services.Common.Intersection;

namespace Services.Common
{
    public interface IAreaToPolygonMapService
    {
        IList<Point> MapAreaPointsToPoints(Area area);
        IList<Segment> ConnectPointsIntoSegments(string areaName, IList<Point> polygonPoints);
        IList<ValidIntersection> GetPolygonValidIntersections(IList<Segment> polygonSegments);
        void MergeSameValidIntersectionPoints(IList<ValidIntersection> validIntersections);
        void SwapPointCoordinates(IEnumerable<Point> points);
    }
}
