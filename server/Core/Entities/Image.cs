using Core.Shared;
using System.Text.Json.Serialization;

namespace Core.Entities;

public class Image : BaseEntity
{
    public required string FullPath { get; set; }
    public required string RelativePath { get; set; }
    public long AlbumId { get; set; }
    [JsonIgnore]
    public Album Album { get; set; }
    public int LikeCount { get; set; }
    public int DislikeCount { get; set; }
    public DateTime UploadedAt { get; set; }
    public IList<Like> Likes { get; set; } = new List<Like>();
}