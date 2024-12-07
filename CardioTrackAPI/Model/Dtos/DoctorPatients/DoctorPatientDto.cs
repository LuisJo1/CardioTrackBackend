using CardioTrackAPI.Model.Dtos.Patient;

namespace CardioTrackAPI.Model.Dtos.DoctorPatients
{
    public class DoctorPatientDto
    {
        public DateTime IssueDate { get; set; }
        public PatientDto? Patient { get; set; }
    }
}
