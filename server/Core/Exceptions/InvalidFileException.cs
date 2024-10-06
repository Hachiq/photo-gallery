namespace Core.Exceptions;

public class InvalidFileException : Exception
{
    public InvalidFileException() : base("Uploaded file is invalid")
    {
    }
}
