namespace Core.DTOs;

public class AlbumDTO
{
    public long Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public long UserId { get; set; }
}
