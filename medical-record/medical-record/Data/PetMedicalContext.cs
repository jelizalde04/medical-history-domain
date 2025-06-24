using Microsoft.EntityFrameworkCore;
using PetMedicalHistoryAPI.Models;

namespace PetMedicalHistoryAPI.Data
{
    public class PetMedicalContext : DbContext
    {
        public PetMedicalContext(DbContextOptions<PetMedicalContext> options) : base(options) { }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
    }
}
