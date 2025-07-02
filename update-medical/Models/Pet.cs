using System;

namespace update_medical.Models
{
    public class Pet
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public string Image { get; set; }
        public DateTime Birthdate { get; set; }
        public string Residence { get; set; }
        public string Gender { get; set; }
        public string Color { get; set; }
        public Guid ResponsibleId { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
