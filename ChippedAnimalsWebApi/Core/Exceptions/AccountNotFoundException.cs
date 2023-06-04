namespace Core.Exceptions
{
    public class AccountNotFoundException : NotFoundException
    {
        public AccountNotFoundException(int? accountId) 
            : base($"Account with id {accountId} not found")
        {
        }
    }
}
