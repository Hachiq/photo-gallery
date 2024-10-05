using Core.Enums;
using Core.Shared;

namespace Core.Entities;

public class User : BaseEntity
{
    public required string Username { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    public Role Role { get; set; } = Role.User;
    public IList<Album> Albums { get; set; } = new List<Album>();
    public IList<Like> Likes { get; set; } = new List<Like>();
}