namespace Services.Common.Intersection
{
    public class Event
    {
        public Point Point { get; init; }
        public EventType Type { get; init; }
        public Segment[] Segments { get; }

        public Event(Point point, EventType type, params Segment[] segments)
        {
            Point = point;
            Segments = segments;
            Type = type;
        }

        public override int GetHashCode()
        {
            int hash = Point.GetHashCode();
            hash = 31 * hash + Type.GetHashCode();
            foreach (Segment segment in Segments)
            {
                hash = hash + segment.GetHashCode();
            }
            return 31 * hash;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            Event e = (Event)obj;
            if (!Point.Equals(e.Point)) return false;
            if (Type != e.Type) return false;
            if (!Segments.All(s => e.Segments.Contains(s))) return false;
            return true;
        }
    }
}
