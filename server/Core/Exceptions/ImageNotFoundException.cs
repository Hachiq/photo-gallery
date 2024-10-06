namespace Core.Exceptions;

public class ImageNotFoundException : Exception
{
    public ImageNotFoundException() : base("Image with provided id does not exist")
    {
    }
}
