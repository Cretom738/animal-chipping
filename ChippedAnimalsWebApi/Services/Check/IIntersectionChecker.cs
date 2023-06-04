using Services.Common.Intersection;

namespace Services.Check
{
    public interface IIntersectionChecker
    {
        void Check(IList<Segment> segments, IList<ValidIntersection> validIntersections);
        void Check(IList<Segment> segments, IList<ValidIntersection> validIntersections, bool isTurned);
        void Clear();
    }
}
