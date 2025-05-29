
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Doctor>> GetDoctors()
        {
            // This method would need to be updated to use the service as well
            // For now, keeping the existing implementation
            static List<Doctor> doctors = new List<Doctor>
            {
                new Doctor{Id=101,Name="Ramu"},
                new Doctor{Id=102,Name="Somu"},
            };
            return Ok(doctors);
        }

        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor([FromBody] DoctorAddRequestDto doctorDto)
        {
            try
            {
                var addedDoctor = await _doctorService.AddDoctor(doctorDto);
                return Created($"/api/doctor/{addedDoctor.Id}", addedDoctor);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding doctor: {ex.Message}");
            }
        }

    }
}