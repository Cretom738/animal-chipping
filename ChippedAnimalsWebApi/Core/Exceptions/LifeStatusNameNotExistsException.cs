namespace Core.Exceptions
{
    public class LifeStatusNameNotExistsException : BadRequestException
    {
        public LifeStatusNameNotExistsException(string lifeStatus)
            : base($"Life status name {lifeStatus} does not exists")
        {
        }
    }
}
