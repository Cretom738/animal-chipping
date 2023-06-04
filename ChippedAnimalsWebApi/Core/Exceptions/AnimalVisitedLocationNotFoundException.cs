namespace Core.Exceptions
{
    public class AnimalVisitedLocationNotFoundException : NotFoundException
    {
        public AnimalVisitedLocationNotFoundException(long? visitedLocationId)
            : base($"Animal visited location with id {visitedLocationId} not found")
        {
        }
    }
}
