namespace Core.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException() : base("User with provided id does not exist")
    {
    }
}
