using DogHouse.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DogHouse.Api.Data
{
    public class DogHouseContext : DbContext
    {
        public DogHouseContext(DbContextOptions<DogHouseContext> opts) : base(opts) { }

        public DbSet<Dog> Dogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dog>()
                .HasKey(d => d.Name);
            modelBuilder.Entity<Dog>()
                .HasIndex(d => d.Name)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }

    public static class DbSeeder
    {
        public static void Seed(DogHouseContext ctx)
        {
            if (!ctx.Dogs.Any())
            {
                ctx.Dogs.AddRange(
                    new Dog { Name = "Neo", Color = "red & amber", TailLength = 22, Weight = 32 },
                    new Dog { Name = "Jessy", Color = "black & white", TailLength = 7, Weight = 14 }
                );
                ctx.SaveChanges();
            }
        }
    }
}