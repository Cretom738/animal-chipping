namespace Core.Exceptions
{
    public class NewChippingPointSameAsFirstVisitedLocationException : BadRequestException
    {
        public NewChippingPointSameAsFirstVisitedLocationException(long? chippingLocationId)
            : base($"New chipping point id {chippingLocationId} same as first visited location")
        {
        }
    }
}
