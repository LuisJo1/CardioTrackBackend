namespace CardioTrackAPI.Model.Dtos.Treatment
{
    public class TreatmentSearchFilters
    {
        public long? ExamId { get; set; }
        public long? PatientId { get; set; }
        public bool GetLatest { get; set; }
        public long? TreatmentId { get; set; }
    }
}
