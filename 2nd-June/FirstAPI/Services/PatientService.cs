using AutoMapper;
using FirstAPI.Interfaces;
using FirstAPI.Models;
using FirstAPI.Models.DTOs;
using FirstAPI.Contexts;


namespace FirstAPI.Services;

public class PatientService
{
    private readonly IMapper _mapper;
    private readonly IEncryptionService _encryptionService;
    private readonly IRepository<string, User> _userRepository;
    private readonly IRepository<int, Patient> _patientRepository;

    public PatientService(IMapper mapper, IEncryptionService encryptionService, IRepository<string, User> userRepository, IRepository<int, Patient> patientRepository)
    {
        _mapper = mapper;
        _encryptionService = encryptionService;
        _userRepository = userRepository;
        _patientRepository = patientRepository;
    }
    public async Task<Patient> AddPatient(PatientAddDto patientDto)
    {
        var user = _mapper.Map<PatientAddDto, User>(patientDto);
        var enc_data = await _encryptionService.EncryptData(new EncryptModel
        {
            Data = patientDto.Password
        });
        user.Password = enc_data.EncryptedData;
        user.HashKey = enc_data.HashKey;
        user.Role = "Patient";
        user = await _userRepository.Add(user);
        if (user == null)
            throw new Exception("Could not add user");
        var patient = _mapper.Map<PatientAddDto, Patient>(patientDto);
        patient.User = user;
        patient = await _patientRepository.Add(patient);
        return patient;
    }
    public async Task<IEnumerable<Patient>> GetAllPatients()
    {
        return await _patientRepository.GetAll();
    }
}