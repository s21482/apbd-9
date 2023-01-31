using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerscriptionsApi.Models;
using PerscriptionsApi.Models.DTO;

namespace PerscriptionsApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/prescriptions")]
    public class PrescriptionsController : Controller
    {
        private readonly MainDbContext _context;

        public PrescriptionsController(MainDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PrescriptionDTO>> GetPrescription(int id)
        {
            var prescription = await _context.Prescriptions.Include(p => p.Patient)
                                                            .Include(p => p.Doctor)
                                                            .Include(p => p.Prescription_Medicaments)
                                                            .ThenInclude(pm => pm.Medicament)
                                                            .FirstOrDefaultAsync(p => p.IdPrescription == id);
            var prescriptionDTO = new PrescriptionDTO
            {
                IdPrescription = prescription.IdPrescription,
                Date = prescription.Date,
                DueDate = prescription.DueDate,
                Patient = new PatientDTO
                {
                    IdPatient = prescription.Patient.IdPatient,
                    FirstName = prescription.Patient.FirstName,
                    LastName = prescription.Patient.LastName,
                    BirthDate = prescription.Patient.BirthDate
                },
                Doctor = new DoctorDTO
                {
                    IdDoctor = prescription.Doctor.IdDoctor,
                    FirstName = prescription.Doctor.FirstName,
                    LastName = prescription.Doctor.LastName,
                    Email = prescription.Doctor.Email
                },
                Medicaments = prescription.Prescription_Medicaments.Select(pm => new MedicamentDTO
                {
                    IdMedicament = pm.Medicament.IdMedicament,
                    Name = pm.Medicament.Name,
                    Description = pm.Medicament.Description,
                    Type = pm.Medicament.Type,
                }).ToList()
            };

            return Ok(prescriptionDTO);
        }

    }
}
