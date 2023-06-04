namespace Services.Common.Intersection
{
    public class Segment
    {
        public string AreaName { get; set; }
        public Point FirstPoint { get; set; }
        public Point SecondPoint { get; set; }
        public double CurrentPosition { get; set; }

        public Segment(string areaName, Point firstPoint, Point secondPoint)
        {
            AreaName = areaName;
            FirstPoint = firstPoint;
            SecondPoint = secondPoint;
        }

        public void RecalculatePosition(double sweepLinePosition)
        {
            double x1 = FirstPoint.X;
            double y1 = FirstPoint.Y;
            double x2 = SecondPoint.X;
            double y2 = SecondPoint.Y;
            CurrentPosition = y1 + (y2 - y1) * (sweepLinePosition - x1) / (x2 - x1);
        }

        public Point? GetIntersectionPoint(Segment other)
        {
            double x1 = FirstPoint.X;
            double y1 = FirstPoint.Y;
            double x2 = SecondPoint.X;
            double y2 = SecondPoint.Y;
            double x3 = other.FirstPoint.X;
            double y3 = other.FirstPoint.Y;
            double x4 = other.SecondPoint.X;
            double y4 = other.SecondPoint.Y;
            double s1x = x2 - x1;
            double s1y = y2 - y1;
            double s2x = x4 - x3;
            double s2y = y4 - y3;
            double s = (s1x * (y1 - y3) - s1y * (x1 - x3)) / (s1x * s2y - s2x * s1y);
            double t = (s2x * (y1 - y3) - s2y * (x1 - x3)) / (s1x * s2y - s2x * s1y);
            if (0 <= s && s <= 1 && 0 <= t && t <= 1)
            {
                double x = x1 + t * s1x;
                double y = y1 + t * s1y;
                return new Point(x, y);
            }
            return null;
        }

        public override int GetHashCode()
        {
            return FirstPoint.GetHashCode() + SecondPoint.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            Segment segment = (Segment)obj;
            if (!FirstPoint.Equals(segment.FirstPoint) 
                && !FirstPoint.Equals(segment.SecondPoint)) return false;
            if (!SecondPoint.Equals(segment.SecondPoint)
                && !SecondPoint.Equals(segment.FirstPoint)) return false;
            return true;
        }

        public override string? ToString()
        {
            return $"Segment {{AreaName: {AreaName}, FirstPoint: {FirstPoint}, SecondPoint: {SecondPoint}}}";
        }

        public void SwapPositions(Segment segment)
        {
            (CurrentPosition, segment.CurrentPosition) = (segment.CurrentPosition, CurrentPosition);
        }
    }
}
