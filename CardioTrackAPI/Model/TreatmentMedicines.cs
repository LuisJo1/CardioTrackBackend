namespace CardioTrackAPI.Model
{
    public class TreatmentMedicines
    {
        public long Id { get; set; }
        public long TreatmentId { get; set; }
        public Treatment? Treatment { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public int TakeEvery { get; set; }
        public int Duration { get; set; }
        public string DurationParatemeter { get; set; } = string.Empty;
    }
}
