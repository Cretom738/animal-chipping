namespace Core.Exceptions
{
    public class GenderNameNotExistsException : BadRequestException
    {
        public GenderNameNotExistsException(string gender)
            : base($"Gender name {gender} does not exists")
        {
        }
    }
}
