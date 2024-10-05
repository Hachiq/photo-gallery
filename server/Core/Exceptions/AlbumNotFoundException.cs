namespace Core.Exceptions;

public class AlbumNotFoundException : Exception
{
    public AlbumNotFoundException() : base("Album with provided id does not exist")
    {
    }
}
