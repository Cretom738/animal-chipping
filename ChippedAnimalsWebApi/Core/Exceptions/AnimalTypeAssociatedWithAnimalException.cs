namespace Core.Exceptions
{
    public class AnimalTypeAssociatedWithAnimalException : BadRequestException
    {
        public AnimalTypeAssociatedWithAnimalException(long? typeId)
            : base($"Animal type with id {typeId} is associated with animal")
        {
        }
    }
}
