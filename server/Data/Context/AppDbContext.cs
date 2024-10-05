using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Data.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>(); 
    public DbSet<Album> Albums => Set<Album>(); 
    public DbSet<Image> Image => Set<Image>(); 
    public DbSet<Like> Likes => Set<Like>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
