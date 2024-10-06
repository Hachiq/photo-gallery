﻿using Core.Shared;

namespace Core.Entities;

public class Image : BaseEntity
{
    public required string FilePath { get; set; }
    public long AlbumId { get; set; }
    public required Album Album { get; set; }
    public int LikeCount { get; set; }
    public int DislikeCount { get; set; }
    public DateTime UploadedAt { get; set; }
    public IList<Like> Likes { get; set; } = new List<Like>();
}