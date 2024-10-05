using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Configurations;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.Property(l => l.IsLike)
            .IsRequired();

        builder.HasOne(l => l.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(l => l.Image)
            .WithMany(i => i.Likes)
            .HasForeignKey(l => l.ImageId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ensure that the same user can like/dislike the same image only once
        builder.HasIndex(l => new { l.UserId, l.ImageId })
            .IsUnique();
    }
}
