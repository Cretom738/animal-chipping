namespace Core.Exceptions
{
    public class ChippingLocationNotFoundException : NotFoundException
    {
        public ChippingLocationNotFoundException(long? pointId)
            : base($"Chipping location with id {pointId} not found")
        {
        }
    }
}
