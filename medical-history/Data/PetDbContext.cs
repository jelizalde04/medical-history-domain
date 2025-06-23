using medical_history.Models;
using Microsoft.EntityFrameworkCore;

namespace medical_history.Data
{
    public class PetDbContext : DbContext
    {
        public PetDbContext(DbContextOptions<PetDbContext> options) : base(options) { }

        public DbSet<Pet> Pets { get; set; }
    }
}
