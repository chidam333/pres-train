
using System.Threading.Tasks;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using FirstAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FirstAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly PatientService _patientService;

    public PatientController(PatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPatient([FromBody] PatientAddDto patientDto)
    {
        if (patientDto == null)
        {
            return BadRequest("Patient data is null");
        }
        try
        {
            var patient = await _patientService.AddPatient(patientDto);
            return Ok(patient);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"error: {ex.Message}");
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Patient>>> GetAll()
    {
        var result = await _patientService.GetAllPatients();
        return Ok(result);
    }

}