using Core.Shared;
using System.Text.Json.Serialization;

namespace Core.Entities;

public class Album : BaseEntity
{
    public required string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public long UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    public IList<Image> Images { get; set; } = new List<Image>();
}