namespace CardioTrackAPI.Model
{
	public class PersonalBackgroundObstetric
	{
		public long Id { get; set; }
		public int GestationWeeks { get; set; }
		public char For { get; set; }
		public bool Caesarean { get; set; }
		public bool Stillbirth { get; set; }
		public bool Abortion { get; set; }
	}
}
