using Microsoft.EntityFrameworkCore;
using task.Models.Entities;

namespace task.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Office> Offices => Set<Office>();
    public DbSet<Phone> Phones => Set<Phone>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Office>().OwnsOne(o => o.Coordinates, c =>
        {
            c.Property(p => p.Latitude).HasColumnName("Latitude");
            c.Property(p => p.Longitude).HasColumnName("Longitude");
        });

        modelBuilder.Entity<Office>().OwnsOne(o => o.Address, a =>
        {
            a.Property(p => p.City).HasColumnName("City");
            a.Property(p => p.ShortAddress).HasColumnName("ShortAddress");
            a.Property(p => p.FullAddress).HasColumnName("FullAddress");
        });

        modelBuilder.Entity<Phone>()
            .HasOne(p => p.Office)
            .WithMany(o => o.Phones)
            .HasForeignKey(p => p.OfficeId);

        modelBuilder.Entity<Phone>().HasIndex(p => p.OfficeId);
        modelBuilder.Entity<Office>().HasIndex(o => o.CityCode);

        modelBuilder.Entity<Office>().Property(o => o.Id).ValueGeneratedNever();
    }
}
