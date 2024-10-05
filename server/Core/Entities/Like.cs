using Core.Shared;

namespace Core.Entities;

public class Like : BaseEntity
{
    public long ImageId { get; set; }
    public required Image Image { get; set; }
    public long UserId { get; set; }
    public required User User { get; set; }
    public bool IsLike { get; set; }
}