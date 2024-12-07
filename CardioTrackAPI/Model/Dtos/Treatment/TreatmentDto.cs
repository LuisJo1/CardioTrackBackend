namespace CardioTrackAPI.Model.Dtos.Treatment
{
    public class TreatmentDto
    {
        public long Id { get; set; }
        public long ExamId { get; set; }
        public long PatientId { get; set; }
        public int Duration { get; set; }
        public string DurationParameter { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime TreatmentStartDate { get; set; }
        public DateTime TreatmentEndDate { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public DateTime AdditionTime { get; set; }
        public List<TreatmentMedicineDto>? TreatmentMedicines { get; set; }
    }
}
