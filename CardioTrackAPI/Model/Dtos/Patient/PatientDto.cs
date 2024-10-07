namespace CardioTrackAPI.Model.Dtos.Patient
{
	public class PatientDto
	{
		public string Email { get; set; } = string.Empty;
		public string Names { get; set; } = string.Empty;
		public string Surnames { get; set; } = string.Empty;
		public string CI { get; set; } = string.Empty;
		public int Age { get; set; }
		public char Genre { get; set;  }
		public DateTime BornDate { get; set; }
	}
}
