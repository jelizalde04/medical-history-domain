using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetMedicalHistoryAPI.Models
{
    [Table("MedicalRecord")]
    public class MedicalRecord
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("petId")]
        public Guid PetId { get; set; }

        [Column("lastVisitDate")]
        public DateTime LastVisitDate { get; set; }

        [Column("weight")]
        public float Weight { get; set; }

        [Column("healthStatus")]
        public string HealthStatus { get; set; }

        [Column("diseases")]
        public string Diseases { get; set; }

        [Column("treatments")]
        public string Treatments { get; set; }

        [Column("vaccinations")]
        public string Vaccinations { get; set; }

        [Column("allergies")]
        public string Allergies { get; set; }

        [Column("specialCare")]
        public string SpecialCare { get; set; }

        [Column("sterilized")]
        public bool Sterilized { get; set; }
    }
}