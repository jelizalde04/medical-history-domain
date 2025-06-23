using medical_history.Data;
using medical_history.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace medical_history.Controllers
{
    [ApiController]
    [Route("api/medicalHistory")] 
    public class MedicalController : ControllerBase
    {
        private readonly PetDbContext _petDbContext;
        private readonly MedicalDbContext _medicalDbContext;

        public MedicalController(PetDbContext petDbContext, MedicalDbContext medicalDbContext)
        {
            _petDbContext = petDbContext;
            _medicalDbContext = medicalDbContext;
        }

        [HttpPost("addMedicalRecord")] 
        public async Task<IActionResult> AddMedicalRecord([FromBody] MedicalRecord record)
        {
            var userId = HttpContext.Items["User"]?.ToString();
            var pet = await _petDbContext.Pets.FindAsync(record.PetId);

            if (pet == null || pet.ResponsibleId != Guid.Parse(userId))
            {
                return Unauthorized(new { error = "Token no válido para esta mascota." });
            }

            _medicalDbContext.MedicalRecords.Add(record);
            await _medicalDbContext.SaveChangesAsync();

            return Ok(record);
        }
    }
}
