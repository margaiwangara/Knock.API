using Knock.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Knock.API.DbContexts
{
  public class KnockContext : DbContext
  {
    public KnockContext(DbContextOptions<KnockContext> options) : base(options){}

    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
      modelBuilder.Entity<User>()
                  .HasIndex(i => i.Email)
                  .IsUnique();

    }
  }
}