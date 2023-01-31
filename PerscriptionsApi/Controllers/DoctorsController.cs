using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerscriptionsApi.Models;
using System.Numerics;

namespace PerscriptionsApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/doctors")]
    public class DoctorsController : Controller
    {
        private readonly MainDbContext _context;

        public DoctorsController(MainDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
        {
            //string g = null;
            //g.ToLower();
            return Ok(await _context.Doctors.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyDoctor(int id, Doctor doctor)
        {
            var doctorFromDb = await _context.Doctors.FindAsync(id);
            if (doctorFromDb == null)
            {
                return NotFound();
            }

            doctorFromDb.FirstName = doctor.FirstName;
            doctorFromDb.LastName = doctor.LastName;
            doctorFromDb.Email = doctor.Email;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
