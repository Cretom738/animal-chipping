namespace Core.Exceptions
{
    public class AnimalVisitedLocationSameAsNextOrPreviousException : BadRequestException
    {
        public AnimalVisitedLocationSameAsNextOrPreviousException(long? visitedLocationId)
            : base($"Visited location with id {visitedLocationId} is same as previous or next")
        {
        }
    }
}
