using Microsoft.EntityFrameworkCore;
using latest_medical.Models;

namespace latest_medical.Data
{
    public class PetContext : DbContext
    {
        public PetContext(DbContextOptions<PetContext> options) : base(options)
        {
        }

        public DbSet<Pet> Pets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pet>(entity =>
            {
                entity.ToTable("Pets");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Species).HasColumnName("species");
                entity.Property(e => e.Breed).HasColumnName("breed");
                entity.Property(e => e.Image).HasColumnName("image");
                entity.Property(e => e.Birthdate).HasColumnName("birthdate");
                entity.Property(e => e.Residence).HasColumnName("residence");
                entity.Property(e => e.Gender).HasColumnName("gender");
                entity.Property(e => e.Color).HasColumnName("color");
                entity.Property(e => e.ResponsibleId).HasColumnName("responsibleId");
                entity.Property(e => e.CreatedAt).HasColumnName("createdAt");
                entity.Property(e => e.UpdatedAt).HasColumnName("updatedAt");
            });
        }
    }
}