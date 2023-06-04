using Core.Exceptions;
using Services.Common.Extensions;
using Services.Common.Intersection;

namespace Services.Check
{
    public class IntersectionChecker : IIntersectionChecker
    {
        readonly PriorityQueue<Event, double> _eventQueue;
        readonly SortedSet<Segment> _sweepLineIntersectedSegments;
        IList<ValidIntersection> _validIntersections = null!;
        bool _isTurned;

        public IntersectionChecker()
        {
            _eventQueue = new PriorityQueue<Event, double>();
            _sweepLineIntersectedSegments = new SortedSet<Segment>(new SegmentsOrderComparer());
        }

        public void Check(IList<Segment> segments, IList<ValidIntersection> validIntersections)
        {
            Check(segments, validIntersections, false);
        }

        public void Check(
            IList<Segment> segments, IList<ValidIntersection> validIntersections, bool isTurned)
        {
            _isTurned = isTurned;
            _validIntersections = validIntersections;
            InitializeEvents(segments);
            HandleEvents();
            Clear();
        }

        public void Clear()
        {
            _validIntersections = null!;
            _eventQueue.Clear();
            _sweepLineIntersectedSegments.Clear();
        }

        void InitializeEvents(IList<Segment> segments)
        {
            foreach (Segment segment in segments)
            {
                _eventQueue.Enqueue(
                    new Event(segment.FirstPoint, EventType.LeftEndpoint, segment),
                    segment.FirstPoint.X);
                _eventQueue.Enqueue(
                    new Event(segment.SecondPoint, EventType.RightEndpoint, segment),
                    segment.SecondPoint.X);
            }
        }

        void HandleEvents()
        {
            while (_eventQueue.TryDequeue(out Event? currentEvent, out double currentPosition))
            {
                if (currentEvent.Type == EventType.LeftEndpoint)
                {
                    HandleLeftEndpointEvent(currentEvent, currentPosition);
                }
                else if (currentEvent.Type == EventType.RightEndpoint)
                {
                    HandleRightEndpointEvent(currentEvent, currentPosition);
                }
            }
        }

        void HandleLeftEndpointEvent(Event leftEndpointEvent, double currentPosition)
        {
            RecalculatePositions(currentPosition);
            Segment eventSegment = leftEndpointEvent.Segments[0];
            _sweepLineIntersectedSegments.Add(eventSegment);
            Segment? segmentAbove = _sweepLineIntersectedSegments.ElementAfter(eventSegment);
            AddPossibleIntersection(eventSegment, segmentAbove, currentPosition);
            Segment? segmentBelow = _sweepLineIntersectedSegments.ElementBefore(eventSegment);
            AddPossibleIntersection(eventSegment, segmentBelow, currentPosition);
        }

        void HandleRightEndpointEvent(Event rightEndpointEvent, double currentPosition)
        {
            Segment eventSegment = rightEndpointEvent.Segments[0];
            Segment? segmentAbove = _sweepLineIntersectedSegments.ElementAfter(eventSegment);
            Segment? segmentBelow = _sweepLineIntersectedSegments.ElementBefore(eventSegment);
            _sweepLineIntersectedSegments.Remove(eventSegment);
            AddPossibleIntersection(segmentAbove, segmentBelow, currentPosition);
        }

        void RecalculatePositions(double sweepLinePosition)
        {
            foreach (Segment currentSegment in _sweepLineIntersectedSegments)
            {
                currentSegment.RecalculatePosition(sweepLinePosition);
            }
        }

        void AddPossibleIntersection(
            Segment? firstSegment, Segment? secondSegment, double currentPosition)
        {
            if (firstSegment == null || secondSegment == null)
            {
                return;
            }
            Point? intersectionPoint = firstSegment.GetIntersectionPoint(secondSegment);
            if (intersectionPoint != null && intersectionPoint.X <= currentPosition)
            {
                if (!IsValidIntersection(firstSegment, secondSegment, intersectionPoint))
                {
                    double intersectionX = _isTurned ? intersectionPoint.Y : intersectionPoint.X;
                    double intersectionY = _isTurned ? intersectionPoint.X : intersectionPoint.Y;
                    if (firstSegment.AreaName == secondSegment.AreaName)
                    {
                        throw new AreaBordersIntersectException(
                            firstSegment.AreaName, intersectionX, intersectionY);
                    }
                    throw new AreaBordersIntersectException(
                        firstSegment.AreaName, secondSegment.AreaName, intersectionX, intersectionY);
                }
            }
        }

        bool IsValidIntersection(
            Segment firstSegment, Segment secondSegment, Point intersectionPoint)
        {
            return _validIntersections.Any(
                vi => vi.Point.Equals(intersectionPoint)
                    && vi.Segments.Contains(firstSegment)
                    && vi.Segments.Contains(secondSegment));
        }
    }
}
