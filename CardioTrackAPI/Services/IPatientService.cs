using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos.Patient;

namespace CardioTrackAPI.Services
{
    public interface IPatientService
	{
		Task<BaseResponse<string>> AddPatientAsync(AddPatientDto addPatientRequest);
		Task<BaseResponse<PatientDto>> GetPatientAsync(long PatientId);
		Task<BaseResponse<string>> DeletePatientAsync(long PatientId);
		Task<BaseResponse<string>> UpdatePatientAsync(long PatientId, UpdatePatientDto updatePatientRequest);
		Task<BaseResponse<string>> PatchPatientAsync(long PatientId, PatchPatientDto patchPatientRequest);
		Task<BaseResponse<SearchWithFilters<PatientDto>>> GetPatientsWithFilters(int sliceIndex, int sliceSize, PatientSearchFilters filters);
	}
}
