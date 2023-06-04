namespace Core.Exceptions
{
    public class AreaNotFoundException : NotFoundException
    {
        public AreaNotFoundException(long? id) 
            : base($"Area with id {id} not found")
        {
        }
    }
}
