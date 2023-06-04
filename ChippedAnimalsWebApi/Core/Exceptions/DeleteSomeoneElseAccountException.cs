namespace Core.Exceptions
{
    public class DeleteSomeoneElseAccountException : ForbiddenException
    {
        public DeleteSomeoneElseAccountException(string someoneEmail, string claimEmail)
            : base($"Account with email {claimEmail} "
                  + $"tried to delete account with email {someoneEmail}")
        {
        }
    }
}
