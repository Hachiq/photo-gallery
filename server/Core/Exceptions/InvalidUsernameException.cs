namespace Core.Exceptions;

public class InvalidUsernameException : Exception
{
	public InvalidUsernameException() : base("User with such username does not exist")
	{
	}
}
