using System;

namespace get_all_medical.Models
{
    public class MedicalRecordCreateDto
    {
        public Guid PetId { get; set; }
        public DateTime LastVisitDate { get; set; }
        public float Weight { get; set; }
        public string HealthStatus { get; set; }
        public string? Diseases { get; set; }
        public string? Treatments { get; set; }
        public string Vaccinations { get; set; }
        public string? Allergies { get; set; }
        public string? SpecialCare { get; set; }
        public bool Sterilized { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}