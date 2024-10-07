namespace CardioTrackAPI.Model.Dtos
{
	public class RecoverPasswordDto
	{
		public string UserEmail { get; set; } = string.Empty;
		public string Token { get; set; } = string.Empty;
		public string NewPassword { get; set; } = string.Empty;
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}
