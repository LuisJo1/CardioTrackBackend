namespace CardioTrackAPI.Model.Dtos.Patient
{
    public class DoctorAttendingDto
    {
        public DateTime IssueDate { get; set; }
        public int examsCount { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string DoctorSpecialty { get; set; } = string.Empty;

    }
}
