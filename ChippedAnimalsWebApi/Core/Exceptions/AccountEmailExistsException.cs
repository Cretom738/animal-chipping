namespace Core.Exceptions
{
    public class AccountEmailExistsException : ConflictException
    {
        public AccountEmailExistsException(string email) 
            : base($"An account with email address {email} already exists")
        {
        }
    }
}
