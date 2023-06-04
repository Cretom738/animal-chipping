namespace Core.Exceptions
{
    public class AnimalTypeNotFoundException : NotFoundException
    {
        public AnimalTypeNotFoundException(long? typeId)
            : base($"Animal type with id {typeId} not found")
        {
        }
    }
}
