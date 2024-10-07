namespace CardioTrackAPI.Model
{
	public class Doctor
	{
		public long Id { get; set; }
		public string Names { get; set; } = string.Empty;
		public string Surnames { get; set; } = string.Empty;
		public string CI { get; set; } = string.Empty;
		public DateTime BornDate { get; set; }
		public long UserId { get; set; }
		public User? User { get; set; }
	}
}