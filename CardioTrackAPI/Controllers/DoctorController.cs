using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos.Doctor;
using CardioTrackAPI.Model.Dtos.DoctorPatients;
using CardioTrackAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CardioTrackAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class DoctorController : ControllerBase
	{
		private readonly IDoctorService _doctorService;
		private readonly IDoctorPatientsService _doctorPatientsService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DoctorController(IDoctorService doctorService, IDoctorPatientsService doctorPatientsService, IHttpContextAccessor httpContextAccessor)
		{
			_doctorService = doctorService;
			_doctorPatientsService = doctorPatientsService;
            _httpContextAccessor = httpContextAccessor;
        }

		[HttpPost]
		[Route("AddDoctor")]
		[Authorize("default-policy", Roles = $"{Roles.Admin}")]
		public async Task<ActionResult<BaseResponse<string>>> AddDoctor(AddDoctorDto addDoctorRequest)
		{
			BaseResponse<string> serviceResp = await _doctorService.AddDoctorAsync(addDoctorRequest);
			return Ok(serviceResp);
		}

		[HttpPut]
		[Route("UpdateDoctor")]
		[Authorize("default-policy", Roles = $"{Roles.Admin}")]
		public async Task<ActionResult<BaseResponse<string>>> UpdateDoctor([FromQuery] long doctorId, [FromBody] UpdateDoctorDto updateDoctorRequest)
		{
			BaseResponse<string> serviceResp = await _doctorService.UpdateDoctorAsync(doctorId, updateDoctorRequest);
			return Ok(serviceResp);
		}

		[HttpPatch]
		[Route("PatchDoctor")]
		[Authorize("default-policy", Roles = $"{Roles.Admin}")]
		public async Task<ActionResult<BaseResponse<string>>> PatchDoctor([FromQuery] long doctorId, [FromBody] PatchDoctorDto patchDoctorRequest)
		{
			BaseResponse<string> serviceResp = await _doctorService.PatchDoctorAsync(doctorId, patchDoctorRequest);
			return Ok(serviceResp);
		}

		[HttpGet]
		[Route("GetDoctor")]
		public async Task<ActionResult<BaseResponse<DoctorDto>>> GetDoctor(long doctorId)
		{
			BaseResponse<DoctorDto> serviceResp = await _doctorService.GetDoctorAsync(doctorId);
			return Ok(serviceResp);
		}

		[HttpDelete]
		[Route("DeleteDoctor")]
		[Authorize("default-policy", Roles = $"{Roles.Admin}")]
		public async Task<ActionResult<BaseResponse<DoctorDto>>> DeleteDoctor(long doctorId)
		{
			BaseResponse<string> serviceResp = await _doctorService.DeleteDoctorAsync(doctorId);
			return Ok(serviceResp);
		}
		[HttpPost]
		[Route("AddPatientToDoctorList")]
		[Authorize("default-policy", Roles = $"{Roles.Doctor}")]
		public async Task<ActionResult<BaseResponse<string>>> AddPatientToDoctorList([FromQuery]long patientId)
		{
			string userId = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Sid)!.Value;
			BaseResponse<string> serviceResp = await _doctorPatientsService.AddPatientToDoctorAsync(patientId, long.Parse(userId));
			return Ok(serviceResp);
		}
		[HttpGet]
		[Route("GetDoctorPatientsWithFilters")]
        [Authorize("default-policy", Roles = $"{Roles.Doctor}")]
		public async Task<IActionResult> GetDoctorPatientsWithFilters(int sliceIndex = 1, int sliceSize = 10, string? patientFullName ="")
		{
            string userId = _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Sid)!.Value;
            BaseResponse<SearchWithFilters<DoctorPatientDto>> serviceResp = await _doctorPatientsService
				.GetDoctorPatientsWithFilters(long.Parse(userId),sliceIndex, sliceSize, new DoctorPatientsSearchFilters()
				{
					PatientFullName = patientFullName
				});
			return Ok(serviceResp);
		}
    }
}
