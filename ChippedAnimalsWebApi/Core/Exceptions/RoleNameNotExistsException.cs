namespace Core.Exceptions
{
    public class RoleNameNotExistsException : BadRequestException
    {
        public RoleNameNotExistsException(string role) 
            : base($"Role name {role} does not exists")
        {
        }
    }
}
