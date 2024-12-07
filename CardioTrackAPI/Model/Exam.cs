namespace CardioTrackAPI.Model
{
	public class Exam
	{
		public long Id { get; set; }
		public long DoctorId { get; set; }
		public Doctor? Doctor { get; set; }
		public DateTime EvaluationDate { get; set; }
		public long PatientId { get; set; }
		public Patient? Patient { get; set; }
		public string InterventionProposed { get; set; } = string.Empty;
		public PersonalBackground? personalBackground { get; set; }
	}
}
