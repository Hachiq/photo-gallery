namespace Core.Exceptions;

public class AlreadyRatedException : Exception
{
    public AlreadyRatedException() : base("The user have already rated this image")
    {
    }
}
