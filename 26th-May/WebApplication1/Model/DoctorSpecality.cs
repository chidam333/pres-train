using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class DoctorSpeciality
{
    [Key]
    public int SerialNumber { get; set; }

    // [ForeignKey("Doctor")]
    public int DoctorId { get; set; }

    // [ForeignKey("Speciality")]
    public int SpecialityId { get; set; }

    public Speciality? Speciality { get; set; }
    public Doctor? Doctor { get; set; }
}