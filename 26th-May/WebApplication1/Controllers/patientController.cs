using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
public class PatientController : ControllerBase
{
    static List<Patient> patients = new List<Patient>
    {
        new Patient{Id=201,Name="ram",Condition="fever",Age=20},
        new Patient{Id=202,Name="bala",Condition="cold",Age=30},
    };

    [HttpGet]
    public ActionResult<IEnumerable<Patient>> GetPatients()
    {
        return Ok(patients);
    }

    [HttpPost]
    public ActionResult<Patient> PostPatient([FromBody] Patient patient)
    {
        patients.Add(patient);
        return Created("", patient);
    }

    [HttpPut]
    [Route("{id:int}")]
    public ActionResult<Patient> PutPatient(int id, [FromBody] Patient patient)
    {
        var existingPatient = patients.FirstOrDefault(p => p.Id == id);
        if (existingPatient == null)
        {
            return NotFound();
        }
        existingPatient.Name = patient.Name;
        return Ok(existingPatient);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public ActionResult DeletePatient(int id)
    {
        var patient = patients.FirstOrDefault(p => p.Id == id);
        if (patient == null)
        {
            return NotFound();
        }
        patients.Remove(patient);
        return NoContent();
    }
}