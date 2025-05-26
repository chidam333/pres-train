using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
public class DoctorController : ControllerBase
{
    static List<Doctor> doctors = new List<Doctor>
    {
        new Doctor{Id=101,Name="Ramu"},
        new Doctor{Id=102,Name="Somu"},
    };
    [HttpGet]
    public ActionResult<IEnumerable<Doctor>> GetDoctors()
    {
        return Ok(doctors);
    }
    [HttpPost]
    public ActionResult<Doctor> PostDoctor([FromBody] Doctor doctor)
    {
        doctors.Add(doctor);
        return Created("", doctor);
    }
    [HttpPut]
    [Route("{id:int}")]
    public ActionResult<Doctor> PutDoctor(int id, [FromBody] Doctor doctor)
    {
        var existingDoctor = doctors.FirstOrDefault(d => d.Id == id);
        if (existingDoctor == null)
        {
            return NotFound();
        }
        existingDoctor.Name = doctor.Name;
        return Ok(existingDoctor);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public ActionResult DeleteDoctor(int id)
    {
        var doctor = doctors.FirstOrDefault(d => d.Id == id);
        if (doctor == null)
        {
            return NotFound();
        }
        doctors.Remove(doctor);
        return NoContent();
    }
}