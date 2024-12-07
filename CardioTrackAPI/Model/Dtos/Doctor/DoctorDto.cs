namespace CardioTrackAPI.Model.Dtos.Doctor
{
    public class DoctorDto
    {
        public long Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Names { get; set; } = string.Empty;
        public string Surnames { get; set; } = string.Empty;
        public string CI { get; set; } = string.Empty;
        public string Specialty { get; set;} = string.Empty;
        public DateTime BornDate { get; set; }
        public string Genre { get; set;} = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
