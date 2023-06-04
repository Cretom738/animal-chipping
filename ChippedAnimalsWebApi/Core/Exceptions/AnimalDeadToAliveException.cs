namespace Core.Exceptions
{
    public class AnimalDeadToAliveException : BadRequestException
    {
        public AnimalDeadToAliveException()
            : base($"Cannot set animal life status from DEAD to ALIVE")
        {
        }
    }
}
