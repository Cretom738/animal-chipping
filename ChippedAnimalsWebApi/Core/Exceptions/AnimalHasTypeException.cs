namespace Core.Exceptions
{
    public class AnimalHasTypeException : ConflictException
    {
        public AnimalHasTypeException(long? animalId, long? typeId)
            : base($"Animal with id {animalId} already has type with id {typeId}")
        {
        }
    }
}
