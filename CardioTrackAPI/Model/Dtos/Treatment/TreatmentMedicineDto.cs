namespace CardioTrackAPI.Model.Dtos.Treatment
{
    public class TreatmentMedicineDto
    {
        public long Id { get; set; }
        public long TreatmentId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public int TakeEvery { get; set; }
        public int Duration { get; set; }
        public string DurationParameter { get; set; } = string.Empty;
    }
}
