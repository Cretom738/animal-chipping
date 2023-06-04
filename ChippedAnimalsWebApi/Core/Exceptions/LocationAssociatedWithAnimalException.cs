namespace Core.Exceptions
{
    public class LocationAssociatedWithAnimalException : BadRequestException
    {
        public LocationAssociatedWithAnimalException(long? pointId)
            : base($"Location with id {pointId} is associated with animal")
        {
        }
    }
}
