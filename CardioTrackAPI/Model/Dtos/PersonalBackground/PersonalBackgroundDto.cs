namespace CardioTrackAPI.Model.Dtos.PersonalBackground
{
    public class PersonalBackgroundDto
    {
        public bool ArterialHypertension { get; set; }
        public bool MyocardialInfarction { get; set; }
        public bool Asthma { get; set; }
        public bool Pneumopathy { get; set; }
        public bool Dyslipidemia { get; set; }
        public bool Angina { get; set; }
        public bool LiverDisease { get; set; }
        public bool Homeopathy { get; set; }
        public bool Stroke { get; set; }
        public bool DiabetesMellitus { get; set; }
        public bool Chagas { get; set; }
        public bool Thyroidopathy { get; set; }
        public bool Surgery { get; set; }
        public string SurgeryType { get; set; } = string.Empty;
        public int GestationWeeks { get; set; }
        public string For { get; set; } = string.Empty;
        public bool Caesarean { get; set; }
        public bool Stillbirth { get; set; }
        public bool Abortion { get; set; }
        public bool MedicineAllergy { get; set; }
        public List<string>? AllergicMedicines { get; set; }
        public bool Toxics { get; set; }
        public List<string>? ToxicsList { get; set; }
        public List<string>? OtherPersonalBackground { get; set; }
        public List<string>? Medicines { get; set; }
        public bool FamilyBackground { get; set; }
        public List<string>? FamilyBackgroundList { get; set; }
    }
}
