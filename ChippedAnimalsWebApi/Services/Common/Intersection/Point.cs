namespace Services.Common.Intersection
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            int hash = X.GetHashCode();
            hash = 31 * hash + Y.GetHashCode();
            return hash;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            Point point = (Point)obj;
            if (X != point.X) return false;
            if (Y != point.Y) return false;
            return true;
        }

        public override string? ToString()
        {
            return $"Point {{X: {X}, Y: {Y}}}";
        }
    }
}
