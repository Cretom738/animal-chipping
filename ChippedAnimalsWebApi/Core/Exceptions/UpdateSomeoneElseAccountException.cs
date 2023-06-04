namespace Core.Exceptions
{
    public class UpdateSomeoneElseAccountException : ForbiddenException
    {
        public UpdateSomeoneElseAccountException(string someoneEmail, string claimEmail)
            : base($"Account with email {claimEmail} " 
                  + $"tried to update account with email {someoneEmail}")
        {
        }
    }
}
