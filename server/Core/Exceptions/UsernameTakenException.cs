namespace Core.Exceptions;

public class UsernameTakenException : Exception
{
    public UsernameTakenException(): base("This username was already taken")
    {
    }
}
