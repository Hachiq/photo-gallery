using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Configurations;

public class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.Property(a => a.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.HasOne(a => a.User)
            .WithMany(u => u.Albums)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(a => a.Images)
            .WithOne(i => i.Album)
            .HasForeignKey(i => i.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
