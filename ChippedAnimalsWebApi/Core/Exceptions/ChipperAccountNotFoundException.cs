namespace Core.Exceptions
{
    public class ChipperAccountNotFoundException : NotFoundException
    {
        public ChipperAccountNotFoundException(int? chipperId)
            : base($"Chipper's account with id {chipperId} not found")
        {
        }
    }
}
