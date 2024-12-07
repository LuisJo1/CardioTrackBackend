namespace CardioTrackAPI.Model.Dtos.PersonalBackground
{
	public class AddPersonalBackgroundObstetricDto
	{
		public int GestationWeeks { get; set; }
		public string For { get; set; } = string.Empty;
		public bool Caesarean { get; set; }
		public bool Stillbirth { get; set; }
		public bool Abortion { get; set; }
	}
}
