namespace Core.Exceptions
{
    public class AreaWithPointCombinationAlreadyExistsException : ConflictException
    {
        public AreaWithPointCombinationAlreadyExistsException() 
            : base("Area with such point combination already exists")
        {
        }
    }
}
