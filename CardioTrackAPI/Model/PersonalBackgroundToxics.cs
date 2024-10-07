namespace CardioTrackAPI.Model
{
	public class PersonalBackgroundToxics
	{
		public long Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public long PersonalBackgroundId { get; set; }
		public PersonalBackground? PersonalBackground { get; set; }
	}
}
