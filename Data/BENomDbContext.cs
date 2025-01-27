using Microsoft.EntityFrameworkCore;
using BENom.Models;

namespace BENom.Data;

public class BENomDbContext : DbContext
{
    public BENomDbContext(DbContextOptions<BENomDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasKey(u => u.Id);
        modelBuilder.Entity<User>().Property(u => u.Username).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
    }
}
