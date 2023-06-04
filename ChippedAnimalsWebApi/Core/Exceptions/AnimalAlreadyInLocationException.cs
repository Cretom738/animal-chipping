namespace Core.Exceptions
{
    public class AnimalAlreadyInLocation : BadRequestException
    {
        public AnimalAlreadyInLocation(long? animalId, long? pointId)
            : base($"Animal with id {animalId} is already in the location with id {pointId}")
        {
        }
    }
}
