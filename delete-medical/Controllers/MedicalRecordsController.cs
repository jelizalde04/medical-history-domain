using delete_medical.Data;
using delete_medical.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetMedicalHistoryAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;

namespace delete_medical.Controllers
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

        /// <summary>
        /// Deletes a medical record if it belongs to the authenticated responsible user.
        /// </summary>
        /// <param name="id">ID of the medical record to delete.</param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMedicalRecord(Guid id)
        {
            // 1. Read the token and extract the userId
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();

            if (string.IsNullOrEmpty(token))
                return Unauthorized("Token not provided.");

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            var responsibleIdString = jsonToken?.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(responsibleIdString) || !Guid.TryParse(responsibleIdString, out var responsibleId))
                return Unauthorized("Responsible ID not found or invalid in the token.");

            // 2. Find the medical record by ID
            var medicalRecord = await _medicalContext.MedicalRecords.FirstOrDefaultAsync(mr => mr.Id == id);

            if (medicalRecord == null)
                return NotFound("Medical record not found.");

            // 3. Find the pet associated with the medical record
            var pet = await _petContext.Pets.FirstOrDefaultAsync(p => p.Id == medicalRecord.PetId);

            if (pet == null)
                return NotFound("Pet associated with the medical record not found.");

            // 4. Validate if the responsible user owns the pet
            if (pet.ResponsibleId != responsibleId)
                return Forbid("You do not have permission to delete the medical history of this pet.");

            // 5. Delete the medical record
            _medicalContext.MedicalRecords.Remove(medicalRecord);
            await _medicalContext.SaveChangesAsync();

            return Ok("Medical record deleted successfully.");
        }
    }
}

public class PetMedicalContext : DbContext
{
    public PetMedicalContext(DbContextOptions<PetMedicalContext> options) : base(options) { }

    public DbSet<Pet> Pets { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
}