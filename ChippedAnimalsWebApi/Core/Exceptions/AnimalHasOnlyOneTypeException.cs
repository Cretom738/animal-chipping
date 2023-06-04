namespace Core.Exceptions
{
    public class AnimalHasOnlyOneSameTypeException : BadRequestException
    {
        public AnimalHasOnlyOneSameTypeException(long? animalId, long? typeId)
            : base($"Animal with id {animalId} has only one type with id {typeId}")
        {
        }
    }
}
