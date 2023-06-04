namespace Core.Exceptions
{
    public class AnimalDoesNotHaveVisitedLocationException : NotFoundException
    {
        public AnimalDoesNotHaveVisitedLocationException(long? animalId, long? visitedLocationId)
            : base($"Animal with id {animalId} does not have " 
                  + $"visited location with id {visitedLocationId}")
        {
        }
    }
}
