namespace Core.Exceptions
{
    public class LocationCoordinatesExistsException : ConflictException
    {
        public LocationCoordinatesExistsException(double? latitude, double? longitude)
            : base($"Location with latitude {latitude} and longitude {longitude} already exists")
        {
        }
    }
}
