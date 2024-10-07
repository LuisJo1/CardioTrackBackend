namespace CardioTrackAPI.Model
{
	public class PersonalBackground
	{
		public long Id { get; set; }
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
		public string? SurgeryType { get; set; }
		public long ObstetricId { get; set; }
		public PersonalBackgroundObstetric? Obstetric {get; set;}
		public bool MedicineAllergy { get; set; }
		public bool Toxics { get; set; }
	}
}
