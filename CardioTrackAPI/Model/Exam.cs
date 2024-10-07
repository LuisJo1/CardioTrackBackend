namespace CardioTrackAPI.Model
{
	public class Exam
	{
		public long Id { get; set; }
		public long DoctorId { get; set; }
		public Doctor? Doctor { get; set; }
		public DateTime IssueDate { get; set; }
		public long PatientId { get; set; }
		public Patient? Patient { get; set; }
		public char PatientGenre { get; set; }
		public int PatientAge { get; set; }
		public long PersonalBackgroundId { get; set; }
		public PersonalBackground? PersonalBackground { get; set; }
	}
}
