using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos.Doctor;

namespace CardioTrackAPI.Services
{
    public interface IDoctorService
	{
		Task<BaseResponse<string>> AddDoctorAsync(AddDoctorDto addDoctorRequest);
		Task<BaseResponse<DoctorDto>> GetDoctorAsync(long doctorId);
		Task<BaseResponse<string>> DeleteDoctorAsync(long doctorId);
		Task<BaseResponse<string>> UpdateDoctorAsync(long doctorId, UpdateDoctorDto updateDoctorRequest);
		Task<BaseResponse<string>> PatchDoctorAsync(long doctorId, PatchDoctorDto patchDoctorRequest);
	}
}
