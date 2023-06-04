namespace Core.Exceptions
{
    public class OldLocationSameAsNewException : BadRequestException
    {
        public OldLocationSameAsNewException(long? pointId)
            : base($"Old location id {pointId} is the same as new's")
        {
        }
    }
}
