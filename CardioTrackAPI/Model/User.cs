using System.ComponentModel.DataAnnotations;

namespace CardioTrackAPI.Model
{
	public class User
	{
		public long Id { get; set; }
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public int RolId { get; set; }
		public Rol? Rol { get; set; }
		public DateTime? LastPasswordRecoveryTimeUTC { get; set; }
	}
}
