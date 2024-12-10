using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos.Treatment;

namespace CardioTrackAPI.Services
{
    public interface ITreatmentService
    {
        Task<BaseResponse<string>> AddTreatmentAsync(AddTreatmentDto addTreatmentDto);
        Task<BaseResponse<SearchWithFilters<TreatmentDto>>> GetTreatmentsWithFilters(int sliceIndex,
            int sliceSize,
            TreatmentSearchFilters searchFilters);

        Task<BaseResponse<string>> UpdateTreatmentMedicines(List<TreatmentMedicineDto> treatmentMedicineDtos);

    }
}
