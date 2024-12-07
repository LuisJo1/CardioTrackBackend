namespace CardioTrackAPI.Model.Dtos.Exam
{
    public class ExamSearchFilters
    {
        public long? ExamId { get; set; }
        public long? DoctorId { get; set; }
        public long? PatientId { get; set; }
    }
}
