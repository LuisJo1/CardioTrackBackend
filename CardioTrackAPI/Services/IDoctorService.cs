using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos.Doctor;
using CardioTrackAPI.Model.Dtos.Patient;

namespace CardioTrackAPI.Services
{
    public interface IDoctorService
	{
		Task<BaseResponse<DoctorDto>> GetDoctorByUserIdAsync(long userId);
		Task<BaseResponse<string>> AddDoctorAsync(AddDoctorDto addDoctorRequest);
		Task<BaseResponse<DoctorDto>> GetDoctorAsync(long doctorId);
		Task<BaseResponse<string>> DeleteDoctorAsync(long doctorId);
		Task<BaseResponse<string>> UpdateDoctorAsync(long doctorId, UpdateDoctorDto updateDoctorRequest);
		Task<BaseResponse<string>> PatchDoctorAsync(long doctorId, PatchDoctorDto patchDoctorRequest);
		Task<BaseResponse<List<DoctorDto>>> GetAllDoctorsAsync();
    }
}
