using CardioTrackAPI.Data;
using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos.DoctorPatients;
using CardioTrackAPI.Model.Dtos.Patient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CardioTrackAPI.Services
{
    public class DoctorPatientsService : IDoctorPatientsService
    {
        private readonly DBContext _dBContext;
        private readonly IPatientService _patientService;

        public DoctorPatientsService(DBContext dBContext, IPatientService patientService)
        {
            _dBContext = dBContext;
            _patientService = patientService;
        }


        public async Task<BaseResponse<string>> AddPatientToDoctorAsync(long patientId, long userId)
        {
            try
            {
                Patient? patient = await _dBContext.Patient.FindAsync(patientId);
                Doctor? doctor = await _dBContext.Doctor.Where(d => d.UserId == userId).FirstOrDefaultAsync();

                if(doctor == null)
                {
                    return BaseResponse<string>.GetError("Coulnd't find a doctor with the given id", HttpStatusCode.NotFound);
                }
                if(patient == null)
                {
                    return BaseResponse<string>.GetError("Coulnd't find a patient with the given id", HttpStatusCode.NotFound);
                }

                if(patient.IsBeingEvaluated)
                {
                    return BaseResponse<string>.GetError("The patient is already being evaluated by a doctor", HttpStatusCode.BadRequest);
                }
                DoctorPatients newDoctorPatient = new DoctorPatients()
                {
                    DoctorId = doctor.Id,
                    IssueDate = DateTime.UtcNow,
                    PatientId = patient.Id,
                };

                await _dBContext.DoctorPatients.AddAsync(newDoctorPatient);
                await _dBContext.SaveChangesAsync();

                BaseResponse<string> patientPatchResp = await _patientService.PatchPatientAsync(patient.Id, new PatchPatientDto()
                {
                    IsBeingEvaluated = true
                });

                if(!patientPatchResp.Success)
                {
                    return BaseResponse<string>.GetError("Coulnd't update the state of the patient", HttpStatusCode.InternalServerError);
                }
                return BaseResponse<string>.GetSuccess("Ok", "Posted", HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.GetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<BaseResponse<SearchWithFilters<DoctorPatientDto>>> GetDoctorPatientsWithFilters(long userId, int sliceIndex, int sliceSize, DoctorPatientsSearchFilters searchFilters)
        {
            try
            {
                Doctor? doctor = await _dBContext.Doctor.Where(d => d.UserId == userId).FirstOrDefaultAsync();
                if (doctor == null)
                {
                    return BaseResponse<SearchWithFilters<DoctorPatientDto>>.GetError("Coulnd't find a doctor", HttpStatusCode.NotFound);
                }
                IQueryable<DoctorPatients> doctorPatientsQuery = _dBContext.DoctorPatients.Include(dp => dp.Doctor)
                    .Include(dp => dp.Patient);

                if(!string.IsNullOrEmpty(searchFilters.PatientFullName))
                {
                    string trimmedFilter = searchFilters.PatientFullName.Replace(" ", "");
                    doctorPatientsQuery = doctorPatientsQuery.
                        Where(dp => (dp.Patient!.Names.Replace(" ", "") + dp.Patient.Surnames.Replace(" ", "")).ToLower().Contains(trimmedFilter.ToLower()));
                }

                int totalResults = await doctorPatientsQuery.CountAsync();

                doctorPatientsQuery = doctorPatientsQuery.Skip((sliceIndex - 1) * sliceSize).Take(sliceSize);

                List<DoctorPatientDto> result = await doctorPatientsQuery.Select(dp => new DoctorPatientDto
                {
                    IssueDate = dp.IssueDate,
                    Patient = new PatientDto()
                    {
                        Id = dp.PatientId,
                        Age = CalculateAge(dp.Patient!.BornDate),
                        CI = dp.Patient.CI,
                        BornDate = dp.Patient.BornDate,
                        Genre = dp.Patient.Genre,
                        IsBeingEvaluated = dp.Patient.IsBeingEvaluated,
                        Names = dp.Patient.Names,
                        Surnames = dp.Patient.Surnames
                    }
                }).ToListAsync();

                return BaseResponse<SearchWithFilters<DoctorPatientDto>>.GetSuccess("Ok", new SearchWithFilters<DoctorPatientDto>(result, totalResults), HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return BaseResponse<SearchWithFilters<DoctorPatientDto>>.GetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
        private static int CalculateAge(DateTime birthDate)
        {
            int age = DateTime.Now.Year - birthDate.Year;
            if (DateTime.Now.Month < birthDate.Month || (DateTime.Now.Month == birthDate.Month && DateTime.Now.Day < birthDate.Day))
            {
                age--;
            }
            return age;
        }
    }
}
