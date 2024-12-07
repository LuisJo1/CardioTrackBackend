namespace CardioTrackAPI.Model
{
    public class Treatment
    {
        public long Id { get; set; }
        public List<TreatmentMedicines>? treatmentMedicines { get; set; }
        public long ExamId { get; set; }
        public Exam? Exam { get; set; }
        public long PatientId { get; set; }
        public Patient? Patient { get; set; }
        public int Duration { get; set; }
        public string DurationParameter { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime TreatmentStartDate { get; set; }
        public DateTime TreatmentEndDate { get; set; }
        public DateTime AdditionTime { get; set; }
    }
}
