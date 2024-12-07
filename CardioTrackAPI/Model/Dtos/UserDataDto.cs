using CardioTrackAPI.Model.Dtos.Doctor;
using CardioTrackAPI.Model.Dtos.Patient;

namespace CardioTrackAPI.Model.Dtos
{
	public class UserDataDto
	{
		public string Id { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public int RolId { get; set; }
		public string RolName { get; set; } = string.Empty;
		public DoctorDto? Doctor { get; set; }
		public PatientDto? Patient { get; set; }
	}
}
