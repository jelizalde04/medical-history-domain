using medical_history.Models;
using Microsoft.EntityFrameworkCore;

namespace medical_history.Data
{
    public class MedicalDbContext : DbContext
    {
        public MedicalDbContext(DbContextOptions<MedicalDbContext> options) : base(options) { }

        public DbSet<MedicalRecord> MedicalRecords { get; set; }
    }
}
