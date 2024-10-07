using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos.Exam;
using CardioTrackAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrackAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ExamController : ControllerBase
	{
		private readonly IExamService _examService;

		public ExamController(IExamService examService)
        {
			_examService = examService;
		}

		[HttpPost]
		[Route("AddExam")]
		[Authorize("default-policy", Roles = $"{Roles.Doctor}, {Roles.Admin}")]
		public async Task<ActionResult<BaseResponse<string>>> AddExam(AddExamDto addExamDto)
		{
			BaseResponse<string> serviceResp = await _examService.AddExamAsync(addExamDto);
			return Ok(serviceResp);
		}
    }
}
