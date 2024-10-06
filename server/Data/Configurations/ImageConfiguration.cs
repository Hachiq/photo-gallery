using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Data.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.Property(i => i.FullPath)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(i => i.RelativePath)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(i => i.UploadedAt)
            .IsRequired();

        builder.Property(i => i.LikeCount)
            .IsRequired();

        builder.Property(i => i.DislikeCount)
            .IsRequired();

        builder.HasOne(i => i.Album)
            .WithMany(a => a.Images)
            .HasForeignKey(i => i.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(i => i.Likes)
            .WithOne(l => l.Image)
            .HasForeignKey(l => l.ImageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
