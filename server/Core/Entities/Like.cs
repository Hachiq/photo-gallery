using Core.Shared;
using System.Text.Json.Serialization;

namespace Core.Entities;

public class Like : BaseEntity
{
    public long ImageId { get; set; }
    [JsonIgnore]
    public Image Image { get; set; }
    public long UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    public bool IsLike { get; set; }
}