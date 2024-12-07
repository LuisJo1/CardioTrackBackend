namespace CardioTrackAPI.Model
{
    public class DoctorPatients
    {
        public long Id { get; set; }
        public long DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public long PatientId { get; set; }
        public Patient? Patient { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
