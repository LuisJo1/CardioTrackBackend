namespace CardioTrackAPI.Model
{
	public class PasswordRecoverToken
	{
		public long Id { get; set; }
		public string Token { get; set; } = string.Empty;
		public DateTime CreationTimeUTC { get; set; }
		public long UserId { get; set; }
		public User? User { get; set; }
		public bool Revoked { get; set; }
	}
}
