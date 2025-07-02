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
            // Validar token JWT
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
                return Unauthorized("Token no proporcionado.");

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jsonToken;
            try
            {
                jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            }
            catch
            {
                return Unauthorized("Token inválido.");
            }

            var responsibleIdString = jsonToken?.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (string.IsNullOrEmpty(responsibleIdString) || !Guid.TryParse(responsibleIdString, out var responsibleId))
            {
                return Unauthorized("ID del responsable no encontrado o no válido en el token.");
            }

            // Validar mascota y responsable
            var pet = await _petContext.Pets.FirstOrDefaultAsync(p => p.Id == dto.PetId);
            if (pet == null)
                return NotFound("Mascota no encontrada.");
            if (pet.ResponsibleId != responsibleId)
                return Unauthorized("No autorizado para editar el historial de esta mascota.");

            // Validar que venga al menos un dato a actualizar
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
                return BadRequest("Debe proporcionar al menos un dato a actualizar.");
            }

            // Buscar el último registro médico por CreatedAt
            var lastRecord = await _medicalContext.MedicalRecords
                                .Where(r => r.PetId == dto.PetId)
                                .OrderByDescending(r => r.CreatedAt)
                                .FirstOrDefaultAsync();

            if (lastRecord == null)
            {
                // No hay registro previo → creamos uno nuevo con los datos recibidos
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
