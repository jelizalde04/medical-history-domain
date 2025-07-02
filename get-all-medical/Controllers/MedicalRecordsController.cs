using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using get_all_medical.Data;
using get_all_medical.Models;
using System.Collections.Generic;

namespace get_all_medical.Controllers
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
        /// Retrieves all medical records of a pet in descending order by CreatedAt.
        /// </summary>
        /// <param name="petId">The ID of the pet.</param>
        /// <returns>All medical records for the pet, ordered descending by creation date.</returns>
        /// <response code="200">Medical records found.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Not found.</response>
        [HttpGet("{petId}")]
        [Authorize]
        [ProducesResponseType(typeof(List<MedicalRecord>), 200)]
        [ProducesResponseType(typeof(object), 401)]
        [ProducesResponseType(typeof(object), 403)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<IActionResult> GetAllMedicalRecords(Guid petId)
        {
            var userIdClaim = User.FindFirst("userId");

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid responsibleId))
            {
                return Unauthorized(new { message = "Invalid or missing userId claim in token." });
            }

            var pet = await _petContext.Pets.FirstOrDefaultAsync(p => p.Id == petId);
            if (pet == null)
            {
                return NotFound(new { message = "Pet not found." });
            }

            if (pet.ResponsibleId != responsibleId)
            {
                return Forbid();
            }

            var records = await _medicalContext.MedicalRecords
                .Where(r => r.PetId == petId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            if (records == null || records.Count == 0)
            {
                return NotFound(new { message = "No medical records found for this pet." });
            }

            return Ok(records);
        }
    }
}

public class PetMedicalContext : DbContext
{
    public PetMedicalContext(DbContextOptions<PetMedicalContext> options) : base(options) { }

    public DbSet<Pet> Pets { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
}
