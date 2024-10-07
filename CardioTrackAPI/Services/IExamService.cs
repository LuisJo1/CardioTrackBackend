using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos.Exam;

namespace CardioTrackAPI.Services
{
	public interface IExamService
	{
		Task<BaseResponse<string>> AddExamAsync(AddExamDto addExamDto);
	}
}
