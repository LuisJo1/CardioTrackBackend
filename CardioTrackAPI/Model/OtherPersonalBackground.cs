namespace CardioTrackAPI.Model
{
	public class OtherPersonalBackground
	{
		public long Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public long PersonalBackgroundId { get; set; }
		public PersonalBackground? personalBackground { get; set; }
	}
}
