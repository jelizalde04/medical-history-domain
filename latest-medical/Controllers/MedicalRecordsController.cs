using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using latest_medical.Data;
using latest_medical.Models;
using System.Collections.Generic;

namespace latest_medical.Controllers
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
        /// Retrieves the last medical record of a pet.
        /// </summary>
        /// <param name="petId">The ID of the pet.</param>
        /// <returns>The last medical record of the pet if found.</returns>
        /// <response code="200">Medical record found.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="403">Forbidden.</response>
        /// <response code="404">Not found.</response>
        [HttpGet("lastest/{petId}")]
        [Authorize]
        [ProducesResponseType(typeof(MedicalRecord), 200)]
        [ProducesResponseType(typeof(object), 401)]
        [ProducesResponseType(typeof(object), 403)]
        [ProducesResponseType(typeof(object), 404)]
        public async Task<IActionResult> GetLastMedicalRecord(Guid petId)
        {
            // Get the userId claim from the authenticated user
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

            var lastRecord = await _medicalContext.MedicalRecords
                .Where(r => r.PetId == petId)
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefaultAsync();

            if (lastRecord == null)
            {
                return NotFound(new { message = "No medical records found for this pet." });
            }

            return Ok(lastRecord);
        }
    }
}