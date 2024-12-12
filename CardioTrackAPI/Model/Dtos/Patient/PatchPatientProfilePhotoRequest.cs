namespace CardioTrackAPI.Model.Dtos.Patient
{
    public class PatchPatientProfilePhotoRequest
    {
        public long PatientId { get; set; }
        public IFormFile? File { get; set; }
    }
}
