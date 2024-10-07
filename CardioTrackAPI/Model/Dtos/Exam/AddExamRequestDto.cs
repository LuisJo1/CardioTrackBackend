using CardioTrackAPI.Model.Dtos.PersonalBackground;

namespace CardioTrackAPI.Model.Dtos.Exam
{
	public class AddExamDto
	{
		public string Names { get; set; } = string.Empty;
		public string Surnames { get; set; } = string.Empty;
		public int Age { get; set; } 
		public string Genre { get; set; } = string.Empty;
		public string InterventionProposed { get; set; } = string.Empty;
		public AddPersonalBackgroundDto?  AddPersonalBackgroundDto { get; set; }
		public long PatientId { get; set; }
	}
}
