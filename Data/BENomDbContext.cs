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

        modelBuilder.Entity<User>().HasKey(u => u.id);
        modelBuilder.Entity<User>().Property(u => u.id_cat_role).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.employee_number).IsRequired().HasMaxLength(6);
        modelBuilder.Entity<User>().Property(u => u.email).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<User>().Property(u => u.password).IsRequired();
    }
}
