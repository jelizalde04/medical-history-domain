using System;

namespace medical_history.Models
{
    public class MedicalRecord
    {
        public Guid Id { get; set; }
        public Guid PetId { get; set; }
        public DateTime LastVisitDate { get; set; }
        public float Weight { get; set; }
        public string HealthStatus { get; set; }
        public string Diseases { get; set; }
    }
}

