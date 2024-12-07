namespace CardioTrackAPI.Model
{
    public class PersonalBackground
    {
        public long Id { get; set; }
        public long ExamId { get; set; }
        public Exam? Exam { get; set; }
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

        public string OtherPersonalBackground { get; set; } = string.Empty;

        public bool Surgery { get; set; }
        public string SurgeryType { get; set; } = string.Empty;



        public int GestationWeeks { get; set; }
        public string For { get; set; }
        public bool Caesarean { get; set; }
        public bool Stillbirth { get; set; }
        public bool Abortion { get; set; }



        public bool MedicinesAllergies { get; set; }
        public string MedicinesAllergiesList { get; set; } = string.Empty;


        public bool Toxics { get; set; }
        public string ToxicsList { get; set; } = string.Empty;


        public string PassMedicines { get; set; } = string.Empty;

        public bool FamilyBackground { get; set; }
        public string FamilyBackgroundList { get; set; } = string.Empty;
    }
}
