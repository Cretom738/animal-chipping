namespace Core.Exceptions
{
    public class AnimalNotHaveTypeException : NotFoundException
    {
        public AnimalNotHaveTypeException(long? animalId, long? typeId)
            : base($"Animal with id {animalId} does not have type with id {typeId}")
        {
        }
    }
}
