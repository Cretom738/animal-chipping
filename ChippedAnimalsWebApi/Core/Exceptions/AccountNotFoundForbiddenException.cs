namespace Core.Exceptions
{
    public class AccountNotFoundForbiddenException : ForbiddenException
    {
        public AccountNotFoundForbiddenException(int? accountId)
            : base($"Account with id {accountId} not found")
        {
        }
    }
}
