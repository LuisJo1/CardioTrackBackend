namespace CardioTrackAPI.Model.Dtos.Doctor
{
    public class AddDoctorDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Names { get; set; } = string.Empty;
        public string Surnames { get; set; } = string.Empty;
        public string CI { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string BornDate { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
