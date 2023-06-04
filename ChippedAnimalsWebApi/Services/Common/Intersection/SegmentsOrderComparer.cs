namespace Services.Common.Intersection
{
    public class SegmentsOrderComparer : IComparer<Segment>
    {
        public int Compare(Segment? first, Segment? second)
        {
            return first!.CurrentPosition.CompareTo(second?.CurrentPosition);
        }
    }
}
