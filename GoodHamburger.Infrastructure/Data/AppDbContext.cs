using GoodHamburger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Menu> Menus => Set<Menu>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Id).ValueGeneratedOnAdd();

                entity.OwnsOne(m => m.Sandwich, sandwich =>
                {
                    sandwich.Property(s => s.Type)
                            .HasColumnName("SandwichType")
                            .HasConversion<int>()
                            .IsRequired();
                });

                entity.Property(m => m.IncludeFrenchFries).IsRequired();
                entity.Property(m => m.IncludeSoda).IsRequired();

                entity.Ignore(m => m.Subtotal);
                entity.Ignore(m => m.Discount);
                entity.Ignore(m => m.TotalPrice);
            });
        }
    }
}
