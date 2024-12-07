using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos.DoctorPatients;

namespace CardioTrackAPI.Services
{
    public interface IDoctorPatientsService
    {
        Task<BaseResponse<string>> AddPatientToDoctorAsync(long patientId, long userId);
        Task<BaseResponse<SearchWithFilters<DoctorPatientDto>>> GetDoctorPatientsWithFilters(long userId, int sliceIndex, int sliceSize, DoctorPatientsSearchFilters searchFilters);
    }
}
