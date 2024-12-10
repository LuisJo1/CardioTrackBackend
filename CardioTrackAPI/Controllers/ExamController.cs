using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos.Exam;
using CardioTrackAPI.Model.Dtos.Treatment;
using CardioTrackAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CardioTrackAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ExamController : ControllerBase
	{
		private readonly IExamService _examService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITreatmentService _treatmentService;

        public ExamController(IExamService examService, IHttpContextAccessor httpContextAccessor, 
			ITreatmentService treatmentService)
        {
			_examService = examService;
            _httpContextAccessor = httpContextAccessor;
            _treatmentService = treatmentService;
        }

		[HttpPost]
		[Route("AddExam")]
		[Authorize("default-policy", Roles = $"{Roles.Doctor}, {Roles.Admin}")]
		public async Task<ActionResult<BaseResponse<string>>> AddExam(AddExamDto addExamDto)
		{
            string userId = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Sid)!.Value;
            BaseResponse<string> serviceResp = await _examService.AddExamAsync(addExamDto, long.Parse(userId));
			return Ok(serviceResp);
		}
		[HttpGet]
		[Route("GetExamsWithFilters")]
		[Authorize("default-policy", Roles = $"{Roles.Doctor}, {Roles.Admin}")]
		public async Task<IActionResult> GetExamsWithFilters(int sliceIndex = 1, int sliceSize = 10, int? doctorId = null, int? patientId = null, int? examId = null)
		{
			BaseResponse<SearchWithFilters<ExamDto>> serviceResp = await _examService
				.GetExamsWithFilters(sliceIndex, sliceSize, new ExamSearchFilters()
				{
					DoctorId = doctorId,
					ExamId = examId,
					PatientId = patientId,
				});
			return Ok(serviceResp);
		}
		[HttpPost]
		[Route("AddTreatment")]
        [Authorize("default-policy", Roles = $"{Roles.Doctor}, {Roles.Admin}")]
		public async Task<ActionResult<BaseResponse<string>>> AddTreatment(AddTreatmentDto addTreatmentDto)
		{
			BaseResponse<string> serviceResp = await _treatmentService.AddTreatmentAsync(addTreatmentDto);
			return Ok(serviceResp);
		}
        [HttpGet]
        [Route("GetTreatmentsWithFilters")]
        [Authorize("default-policy", Roles = $"{Roles.Doctor}, {Roles.Admin}, {Roles.Patient}")]
        public async Task<IActionResult> GetTreatmentsWithFilters(int sliceIndex = 1, int sliceSize = 10, int? patientId = null, int? examId = null, bool getLatest = false, int? treatmentId = 0, bool getAll = false)
        {
            BaseResponse<SearchWithFilters<TreatmentDto>> serviceResp = await _treatmentService
                .GetTreatmentsWithFilters(sliceIndex, sliceSize, new TreatmentSearchFilters()
                {
                    ExamId = examId,
                    PatientId = patientId,
					GetLatest = getLatest,
					TreatmentId = treatmentId,
					GetAll = getAll
                });
            return Ok(serviceResp);
        }
    }
}
