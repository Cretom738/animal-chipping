using Core.Exceptions;
using Core.Models;
using Services.Common.Intersection;

namespace Services.Common
{
    public class AreaToPolygonMapService : IAreaToPolygonMapService
    {
        public IList<Point> MapAreaPointsToPoints(Area area)
        {
            return area.AreaPoints
                .Select(ap => new Point(ap.Longitude, ap.Latitude))
                .ToList();
        }

        public IList<Segment> ConnectPointsIntoSegments(string areaName, IList<Point> polygonPoints)
        {
            IList<Segment> segments = new List<Segment>(polygonPoints.Count);
            Segment currentSegment = new Segment(areaName, polygonPoints[0], polygonPoints[1]);
            Point nextPoint;
            for (int i = 1; i < polygonPoints.Count - 1; i++)
            {
                nextPoint = polygonPoints[i + 1];
                if (AreOnSameLine(
                    currentSegment.FirstPoint, currentSegment.SecondPoint, nextPoint))
                {
                    currentSegment.SecondPoint = nextPoint;
                }
                else
                {
                    segments.Add(currentSegment);
                    currentSegment = new Segment(areaName, polygonPoints[i], nextPoint);
                }
            }
            segments.Add(currentSegment);
            segments.Add(new Segment(areaName, polygonPoints[polygonPoints.Count - 1], polygonPoints[0]));
            if (segments.Count < 3)
            {
                throw new AreaPointsAreOnSameLineException(areaName);
            }
            return segments;
        }

        public IList<ValidIntersection> GetPolygonValidIntersections(IList<Segment> polygonSegments)
        {
            IList<ValidIntersection> validIntersections = new List<ValidIntersection>();
            for (int i = 0; i < polygonSegments.Count - 1; i++)
            {
                validIntersections.Add(new ValidIntersection(
                    polygonSegments[i].SecondPoint, polygonSegments[i], polygonSegments[i + 1]));
            }
            validIntersections.Add(new ValidIntersection(
                polygonSegments[polygonSegments.Count - 1].SecondPoint,
                polygonSegments[polygonSegments.Count - 1],
                polygonSegments[0]));
            return validIntersections;
        }

        public void MergeSameValidIntersectionPoints(IList<ValidIntersection> validIntersections)
        {
            for (int i = 0; i < validIntersections.Count; i++)
            {
                for (int j = i + 1; j < validIntersections.Count; j++)
                {
                    if (validIntersections[i].Point.Equals(validIntersections[j].Point))
                    {
                        validIntersections[i].Segments = validIntersections[i].Segments
                            .Union(validIntersections[j].Segments)
                            .ToList();
                        validIntersections.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        public void SwapPointCoordinates(IEnumerable<Point> points)
        {
            foreach (Point point in points)
            {
                (point.X, point.Y) = (point.Y, point.X);
            }
        }

        bool AreOnSameLine(Point firstPoint, Point secondPoint, Point thirdPoint)
        {
            double x1 = firstPoint.X;
            double y1 = firstPoint.Y;
            double x2 = secondPoint.X;
            double y2 = secondPoint.Y;
            double x3 = thirdPoint.X;
            double y3 = thirdPoint.Y;
            return (y2 - y1) * (x3 - x1) == (x2 - x1) * (y3 - y1);
        }
    }
}
