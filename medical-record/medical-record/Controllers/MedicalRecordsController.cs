using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetMedicalHistoryAPI.Data;
using PetMedicalHistoryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;

namespace PetMedicalHistoryAPI.Controllers
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateMedicalRecord([FromBody] MedicalRecordCreateDto dto)
        {
            // Decode the JWT token and extract the controller ID
            var token = HttpContext.Request.Headers["Authorization"].ToString().Trim();
            if (string.IsNullOrEmpty(token)) return Unauthorized("Token no proporcionado.");

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var responsibleIdString = jsonToken?.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(responsibleIdString) || !Guid.TryParse(responsibleIdString, out var responsibleId))
            {
                return Unauthorized("ID del responsable no encontrado o no válido en el token.");
            }

            // Validate if the pet exists and belongs to the person responsible in the pet database
            var pet = await _petContext.Pets.FirstOrDefaultAsync(p => p.Id == dto.PetId);
            if (pet == null || pet.ResponsibleId != responsibleId)
            {
                return Unauthorized("No autorizado para editar el historial de esta mascota.");
            }

            // Create the medical record with all fields
            var medicalRecord = new MedicalRecord
            {
                Id = Guid.NewGuid(),
                PetId = dto.PetId,
                LastVisitDate = dto.LastVisitDate,
                Weight = dto.Weight,
                HealthStatus = dto.HealthStatus,
                Diseases = dto.Diseases,
                Treatments = dto.Treatments,
                Vaccinations = dto.Vaccinations,
                Allergies = dto.Allergies,
                SpecialCare = dto.SpecialCare,
                Sterilized = dto.Sterilized
            };

            _medicalContext.MedicalRecords.Add(medicalRecord);
            await _medicalContext.SaveChangesAsync();

            return Ok(medicalRecord);
        }

    }
}

public class PetMedicalContext : DbContext
{
    public PetMedicalContext(DbContextOptions<PetMedicalContext> options) : base(options) { }

    public DbSet<Pet> Pets { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
}