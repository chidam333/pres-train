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