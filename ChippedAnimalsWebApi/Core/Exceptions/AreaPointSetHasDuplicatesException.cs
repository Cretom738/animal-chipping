namespace Core.Exceptions
{
    public class AreaPointSetHasDuplicatesException : BadRequestException
    {
        public AreaPointSetHasDuplicatesException() 
            : base($"Area point set has duplicates")
        {
        }
    }
}
