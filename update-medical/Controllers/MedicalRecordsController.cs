using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using update_medical.Data;
using update_medical.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace update_medical.Controllers
{
    [Route("medical")]
    [ApiController]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly PetMedicalContext _medicalContext;
        private readonly PetContext _petContext;

        public MedicalRecordsController(PetMedicalContext medicalContext, PetContext petContext)
        {
            _medicalContext = medicalContext;
            _petContext = petContext;
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> UpdateMedicalRecord([FromBody] MedicalRecordUpdateDto dto)
        {
            // Validate JWT token
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
                return Unauthorized("Token not provided.");

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jsonToken;
            try
            {
                jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            }
            catch
            {
                return Unauthorized("Invalid token.");
            }

            var responsibleIdString = jsonToken?.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (string.IsNullOrEmpty(responsibleIdString) || !Guid.TryParse(responsibleIdString, out var responsibleId))
            {
                return Unauthorized("Invalid userId in token.");
            }

            // Validate pet and responsible
            var pet = await _petContext.Pets.FirstOrDefaultAsync(p => p.Id == dto.PetId);
            if (pet == null)
                return NotFound("Pet not found.");
            if (pet.ResponsibleId != responsibleId)
                return Unauthorized("Not authorized for this pet.");

            // Validate at least one field to update
            if (
                dto.LastVisitDate == null &&
                dto.Weight == null &&
                dto.HealthStatus == null &&
                dto.Diseases == null &&
                dto.Treatments == null &&
                dto.Vaccinations == null &&
                dto.Allergies == null &&
                dto.SpecialCare == null &&
                dto.Sterilized == null
            )
            {
                return BadRequest("At least one field must be provided.");
            }

            // Find last medical record by CreatedAt
            var lastRecord = await _medicalContext.MedicalRecords
                                .Where(r => r.PetId == dto.PetId)
                                .OrderByDescending(r => r.CreatedAt)
                                .FirstOrDefaultAsync();

            if (lastRecord == null)
            {
                // No previous record → create new one
                var newRecord = new MedicalRecord
                {
                    Id = Guid.NewGuid(),
                    PetId = dto.PetId,
                    LastVisitDate = dto.LastVisitDate?.ToUniversalTime() ?? DateTime.UtcNow,
                    Weight = dto.Weight ?? 0,
                    HealthStatus = dto.HealthStatus,
                    Diseases = dto.Diseases,
                    Treatments = dto.Treatments,
                    Vaccinations = dto.Vaccinations,
                    Allergies = dto.Allergies,
                    SpecialCare = dto.SpecialCare,
                    Sterilized = dto.Sterilized ?? false,
                    CreatedAt = DateTime.UtcNow
                };

                _medicalContext.MedicalRecords.Add(newRecord);
                await _medicalContext.SaveChangesAsync();

                return Ok(newRecord);
            }
            else
            {
                var lastVisitDateSafe = DateTime.SpecifyKind(lastRecord.LastVisitDate, DateTimeKind.Utc);

                // Create updated record
                var updatedRecord = new MedicalRecord
                {
                    Id = Guid.NewGuid(),
                    PetId = lastRecord.PetId,
                    LastVisitDate = dto.LastVisitDate.HasValue
                        ? dto.LastVisitDate.Value.ToUniversalTime()
                        : lastVisitDateSafe,
                    Weight = dto.Weight.HasValue ? dto.Weight.Value : lastRecord.Weight,
                    HealthStatus = dto.HealthStatus ?? lastRecord.HealthStatus,
                    Diseases = dto.Diseases ?? lastRecord.Diseases,
                    Treatments = dto.Treatments ?? lastRecord.Treatments,
                    Vaccinations = dto.Vaccinations ?? lastRecord.Vaccinations,
                    Allergies = dto.Allergies ?? lastRecord.Allergies,
                    SpecialCare = dto.SpecialCare ?? lastRecord.SpecialCare,
                    Sterilized = dto.Sterilized.HasValue ? dto.Sterilized.Value : lastRecord.Sterilized,
                    CreatedAt = DateTime.UtcNow
                };

                // VALIDATION: Check for duplicate records
                if (updatedRecord.LastVisitDate.Date == lastVisitDateSafe.Date &&
                    updatedRecord.Weight == lastRecord.Weight &&
                    updatedRecord.HealthStatus == lastRecord.HealthStatus &&
                    updatedRecord.Diseases == lastRecord.Diseases &&
                    updatedRecord.Treatments == lastRecord.Treatments &&
                    updatedRecord.Vaccinations == lastRecord.Vaccinations &&
                    updatedRecord.Allergies == lastRecord.Allergies &&
                    updatedRecord.SpecialCare == lastRecord.SpecialCare &&
                    updatedRecord.Sterilized == lastRecord.Sterilized)
                {
                    return BadRequest(new { 
                        message = "Cannot create duplicate record.",
                        details = "All fields are identical to last record."
                    });
                }

                _medicalContext.MedicalRecords.Add(updatedRecord);
                await _medicalContext.SaveChangesAsync();

                return Ok(updatedRecord);
            }

        }
    }
}

public class PetMedicalContext : DbContext
{
    public PetMedicalContext(DbContextOptions<PetMedicalContext> options) : base(options) { }

    public DbSet<Pet> Pets { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
}
