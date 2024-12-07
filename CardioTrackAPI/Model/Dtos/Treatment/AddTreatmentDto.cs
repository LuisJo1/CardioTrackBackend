namespace CardioTrackAPI.Model.Dtos.Treatment
{
    public class AddTreatmentDto
    {
        public long ExamId { get; set; }
        public long PatientId { get; set; }
        public int Duration { get; set; }
        public string DurationParameter { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime TreatmentStartDate { get; set; }
        public DateTime TreatmentEndDate { get; set; }
        public List<TreatmentMedicineDto> TreatmentMedicines { get; set; } = new List<TreatmentMedicineDto>();
    }
}
