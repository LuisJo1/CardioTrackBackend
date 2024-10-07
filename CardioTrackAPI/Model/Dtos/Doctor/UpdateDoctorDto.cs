namespace CardioTrackAPI.Model.Dtos.Doctor
{
    public class UpdateDoctorDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Names { get; set; } = string.Empty;
        public string Surnames { get; set; } = string.Empty;
        public string CI { get; set; } = string.Empty;
        public string BornDate { get; set; } = string.Empty;
    }
}
