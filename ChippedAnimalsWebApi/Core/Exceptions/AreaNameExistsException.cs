namespace Core.Exceptions
{
    public class AreaNameExistsException : ConflictException
    {
        public AreaNameExistsException(string name) 
            : base($"An area with name {name} already exists")
        {
        }
    }
}
