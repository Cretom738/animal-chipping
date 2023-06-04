namespace Core.Exceptions
{
    public class AnimalTypeNameExistsException : ConflictException
    {
        public AnimalTypeNameExistsException(string type)
            : base($"Animal type with type name {type} already exists")
        {
        }
    }
}
