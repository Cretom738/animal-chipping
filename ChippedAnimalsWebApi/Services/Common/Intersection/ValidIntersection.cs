namespace Services.Common.Intersection
{
    public class ValidIntersection
    {
        public Point Point { get; init; }
        public IList<Segment> Segments { get; set; }

        public ValidIntersection(Point point, params Segment[] segments)
        {
            Point = point;
            Segments = segments;
        }

        public override string? ToString()
        {
            return $"ValidIntersection {{Point: {Point}, Segments:\n{string.Join(",\n", Segments)}}}";
        }
    }
}
