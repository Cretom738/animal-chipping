namespace Core.Exceptions
{
    public class AnimalAssosiatedWithVisitedLocationException : BadRequestException
    {
        public AnimalAssosiatedWithVisitedLocationException(long? animalId)
            : base($"Animal with id {animalId} is assosiated with animal visited location")
        {
        }
    }
}
