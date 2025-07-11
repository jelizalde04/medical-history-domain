﻿using Microsoft.EntityFrameworkCore;
using update_medical.Models;

namespace update_medical.Data
{
    public class PetMedicalContext : DbContext
    {
        public PetMedicalContext(DbContextOptions<PetMedicalContext> options) : base(options) { }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
    }
}
