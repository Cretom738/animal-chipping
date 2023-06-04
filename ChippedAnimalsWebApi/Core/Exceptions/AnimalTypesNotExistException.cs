namespace Core.Exceptions
{
    public class AnimalTypesNotExistException : NotFoundException
    {
        public AnimalTypesNotExistException(IEnumerable<long> typeIds)
            : base($"Animal types with ids {string.Join(", ", typeIds)} do not exist")
        {
        }
    }
}
