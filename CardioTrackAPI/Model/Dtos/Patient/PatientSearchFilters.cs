namespace CardioTrackAPI.Model.Dtos.Patient
{
	public class PatientSearchFilters
	{
		public string? CI { get; set; }
		public string? FullName { get; set; }
		public string? SearchTerm { get; set; }
		public bool? IsBeingEvaluated { get; set; }

	}
}
