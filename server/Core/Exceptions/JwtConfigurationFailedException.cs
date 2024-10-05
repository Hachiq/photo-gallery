namespace Core.Exceptions;

public class JwtConfigurationFailedException : Exception
{
    public JwtConfigurationFailedException() : base("Access Token configuration failed")
    {
    }
}
