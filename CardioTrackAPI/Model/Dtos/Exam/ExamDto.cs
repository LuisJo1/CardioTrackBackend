using CardioTrackAPI.Model.Dtos.PersonalBackground;

namespace CardioTrackAPI.Model.Dtos.Exam
{
    public class ExamDto
    {
        public long Id { get; set; }
        public string InterventionProposed { get; set; } = string.Empty;
        public PersonalBackgroundDto? PersonalBackground { get; set; }
        public DateTime EvaluationDate { get; set; }
    }
}
