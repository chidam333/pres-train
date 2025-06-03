namespace FirstAPI.Controllers;

using FirstAPI.Models;
using FirstAPI.Auth;
using FirstAPI.Contexts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

[ApiController]
[Route("/api/[controller]")]
public class CancelAppointmentController : ControllerBase
{
    [MinExp(3)]
    [HttpPost]
    public async Task<ActionResult<Doctor>> CancelAppointment()
    {
        Console.WriteLine("Cancelling appointment for doctor.");
        return Ok();
    }
}


[ApiController]
[Route("/api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly ClinicContext _clientContext;
    public AppointmentController(ClinicContext clientContext)
    {
        _clientContext = clientContext;
    }

    [HttpPost]
    public async Task<ActionResult<Doctor>> Appointment(Appointmnet appointmnet)
    {
        Console.WriteLine("Creating appointment for doctor.");
        if (appointmnet == null || appointmnet.DoctorId <= 0)
        {
            return BadRequest("Invalid appointment data.");
        }

        var doctor = await _clientContext.Doctors.FindAsync(appointmnet.DoctorId);
        if (doctor == null)
        {
            return NotFound("Doctor not found.");
        }

        return Ok(doctor);
    }
}