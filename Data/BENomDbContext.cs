using Microsoft.EntityFrameworkCore;
using BENom.Models;

namespace BENom.Data;

public class BENomDbContext : DbContext
{
    public BENomDbContext(DbContextOptions<BENomDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Sublocation> Sublocations { get; set; }
    public DbSet<Witness> Witnesses { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<Requester> Requesters { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Reason> Reasons { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Via> Vias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasKey(u => u.id);
        modelBuilder.Entity<User>().Property(u => u.id_role).IsRequired();
        modelBuilder.Entity<User>().Property(u => u.employee_number).IsRequired().HasMaxLength(6);
        modelBuilder.Entity<User>().Property(u => u.email).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<User>().Property(u => u.password).IsRequired();
    }
}
