using Microsoft.EntityFrameworkCore;
using latest_medical.Models;

namespace latest_medical.Data
{
    public class PetMedicalContext : DbContext
    {
        public PetMedicalContext(DbContextOptions<PetMedicalContext> options) : base(options) { }

        public DbSet<MedicalRecord> MedicalRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                entity.ToTable("MedicalRecord");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.PetId).HasColumnName("petId");
                entity.Property(e => e.LastVisitDate).HasColumnName("lastVisitDate");
                entity.Property(e => e.Weight).HasColumnName("weight");
                entity.Property(e => e.HealthStatus).HasColumnName("healthStatus");
                entity.Property(e => e.Diseases).HasColumnName("diseases").IsRequired(false);
                entity.Property(e => e.Treatments).HasColumnName("treatments").IsRequired(false);
                entity.Property(e => e.Vaccinations).HasColumnName("vaccinations");
                entity.Property(e => e.Allergies).HasColumnName("allergies").IsRequired(false);
                entity.Property(e => e.SpecialCare).HasColumnName("specialCare").IsRequired(false);
                entity.Property(e => e.Sterilized).HasColumnName("sterilized");
                entity.Property(e => e.CreatedAt).HasColumnName("createdAt");
            });
        }
    }
}
