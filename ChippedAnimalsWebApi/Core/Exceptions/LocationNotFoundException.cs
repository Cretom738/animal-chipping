namespace Core.Exceptions
{
    public class LocationNotFoundException : NotFoundException
    {
        public LocationNotFoundException(long? pointId)
            : base($"Location with id {pointId} not found")
        {
        }

        public LocationNotFoundException(double? latitude, double? longitude)
            : base($"Location with latitude {latitude} and longitude {longitude} not found")
        {
        }
    }
}
