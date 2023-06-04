namespace Core.Exceptions
{
    public class AccountAssosiatedWithAnimalException : BadRequestException
    {
        public AccountAssosiatedWithAnimalException(int? accountId)
            : base($"Account with id {accountId} is associated with animal")
        {
        }
    }
}
