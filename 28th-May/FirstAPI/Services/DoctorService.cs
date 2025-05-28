using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;

namespace FirstAPI.Services
{
    public class DoctorService : IDoctorService
    {   
        private readonly IRepository<int, Doctor> _doctorRepository;
        private readonly IRepository<int, Speciality> _specialityRepository;
        private readonly IRepository<int, DoctorSpeciality> _doctorSpecialityRepository;

        public DoctorService(IRepository<int, Doctor> doctorRepository,
                           IRepository<int, Speciality> specialityRepository,
                           IRepository<int, DoctorSpeciality> doctorSpecialityRepository)
        {
            _doctorRepository = doctorRepository;
            _specialityRepository = specialityRepository;
            _doctorSpecialityRepository = doctorSpecialityRepository;
        }

        public async Task<Doctor> GetDoctByName(string name)
        {
            var allDoctors = await _doctorRepository.GetAll();
            return allDoctors.FirstOrDefault(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<ICollection<Doctor>> GetDoctorsBySpeciality(string speciality)
        {
            var allSpecialities = await _specialityRepository.GetAll();
            var targetSpeciality = allSpecialities.FirstOrDefault(s => s.Name.Equals(speciality, StringComparison.OrdinalIgnoreCase));
            
            if (targetSpeciality == null)
                return new List<Doctor>();

            var doctorSpecialities = await _doctorSpecialityRepository.GetAll();
            var doctorIds = doctorSpecialities
                .Where(ds => ds.SpecialityId == targetSpeciality.Id)
                .Select(ds => ds.DoctorId);

            var allDoctors = await _doctorRepository.GetAll();
            return allDoctors.Where(d => doctorIds.Contains(d.Id)).ToList();
        }

        public async Task<Doctor> AddDoctor(DoctorAddRequestDto doctorDto)
        {
            var doctor = new Doctor
            {
                Name = doctorDto.Name,
                YearsOfExperience = doctorDto.YearsOfExperience,
                Status = "Active"
            };

            var addedDoctor = await _doctorRepository.Add(doctor);

            if (doctorDto.Specialities?.Any() == true)
            {
                var allSpecialities = await _specialityRepository.GetAll();
                
                foreach (var specialityDto in doctorDto.Specialities)
                {
                    var specialityId = await GetOrCreateSpecialityId(specialityDto.Name, allSpecialities);
                    
                    await _doctorSpecialityRepository.Add(new DoctorSpeciality
                    {
                        DoctorId = addedDoctor.Id,
                        SpecialityId = specialityId
                    });
                }
            }

            return addedDoctor;
        }

        private async Task<int> GetOrCreateSpecialityId(string specialityName, IEnumerable<Speciality> existingSpecialities)
        {
            var existingSpeciality = existingSpecialities.FirstOrDefault(s => 
                s.Name.Equals(specialityName, StringComparison.OrdinalIgnoreCase));
            
            if (existingSpeciality != null)
                return existingSpeciality.Id;

            var newSpeciality = await _specialityRepository.Add(new Speciality
            {
                Name = specialityName,
                Status = "Active"
            });
            
            return newSpeciality.Id;
        }
    }
}