using CardioTrackAPI.Model.Dtos.PersonalBackground;

namespace CardioTrackAPI.Model.Dtos.Exam
{
	public class AddExamDto
	{
		public string InterventionProposed { get; set; } = string.Empty;
		public AddPersonalBackgroundDto?  AddPersonalBackgroundDto { get; set; }
		public long PatientId { get; set; }
	}
}
