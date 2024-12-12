using CardioTrackAPI.Model.Dtos.Patient;
using CardioTrackAPI.Model;
using CardioTrackAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CTalk.AppServices;

namespace CardioTrackAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class PatientController : ControllerBase
	{
		private readonly IPatientService _patientService;
        private readonly IStorageService _storageService;

        public PatientController(IPatientService patientService, IStorageService storageService)
		{
			_patientService = patientService;
            _storageService = storageService;
        }

		[HttpPost]
		[Route("AddPatient")]
        public async Task<ActionResult<BaseResponse<string>>> AddPatient(AddPatientDto addPatientRequest)
		{
			BaseResponse<string> serviceResp = await _patientService.AddPatientAsync(addPatientRequest);
			return Ok(serviceResp);
		}

		[HttpPut]
		[Route("UpdatePatient")]
		[Authorize("default-policy", Roles = $"{Roles.Doctor}, {Roles.Admin}")]
		public async Task<ActionResult<BaseResponse<string>>> UpdatePatient([FromQuery] long patientId, [FromBody] UpdatePatientDto updatePatientRequest)
		{
			BaseResponse<string> serviceResp = await _patientService.UpdatePatientAsync(patientId, updatePatientRequest);
			return Ok(serviceResp);
		}

		[HttpPatch]
		[Route("PatchPatient")]
		[Authorize("default-policy", Roles = $"{Roles.Doctor}, {Roles.Admin}")]
		public async Task<ActionResult<BaseResponse<string>>> PatchPatient([FromQuery] long patientId, [FromBody] PatchPatientDto patchPatientRequest)
		{
			BaseResponse<string> serviceResp = await _patientService.PatchPatientAsync(patientId, patchPatientRequest);
			return Ok(serviceResp);
		}

		[HttpGet]
		[Route("GetPatient")]
		public async Task<ActionResult<BaseResponse<PatientDto>>> GetPatient(long patientId)
		{
			BaseResponse<PatientDto> serviceResp = await _patientService.GetPatientAsync(patientId);
			return Ok(serviceResp);
		}
		[HttpGet]
        [Authorize("default-policy", Roles = $"{Roles.Doctor}, {Roles.Admin}")]
        [Route("GetPatientWithFilters")]
		public async Task<IActionResult> GetPatientWithFilters([FromQuery]int sliceIndex = 1, [FromQuery]int sliceSize = 10, string? ci = "", string? fullName = "", bool? isBeingEvaluated = null, string? searchTerm = "")
		{
			BaseResponse<SearchWithFilters<PatientDto>> serviceResp = await _patientService.GetPatientsWithFilters(sliceIndex, sliceSize, new PatientSearchFilters
			{
				CI = ci,
				FullName = fullName,
				SearchTerm = searchTerm,
				IsBeingEvaluated = isBeingEvaluated
			});

			return Ok(serviceResp);
		}


		[HttpDelete]
		[Route("DeletePatient")]
		[Authorize("default-policy", Roles = $"{Roles.Admin}")]
		public async Task<ActionResult<BaseResponse<PatientDto>>> DeletePatient(long patientId)
		{
			BaseResponse<string> serviceResp = await _patientService.DeletePatientAsync(patientId);
			return Ok(serviceResp);
		}
		[HttpGet]
		[Route("GetDoctorAttending")]
		[Authorize("default-policy")]
		public async Task<ActionResult<BaseResponse<DoctorAttendingDto>>> GetDoctorAttending([FromQuery]long patientId)
        {
			BaseResponse<DoctorAttendingDto> serviceResp = await _patientService.GetDoctorAttending(patientId);
            return Ok(serviceResp);
        }
		[HttpPatch]
		[Route("PatchPatientProfilePhoto")]
		[Authorize("default-policy", Roles = $"{Roles.Doctor}, {Roles.Admin}, {Roles.Patient}")]
		public async Task<IActionResult> PatchPatientProfilePhoto(PatchPatientProfilePhotoRequest request)
		{
			BaseResponse<string> serviceResp = await _storageService.UploadImagesAsync(new List<IFormFile>() { request.File! }, request.PatientId);
			return Ok(serviceResp);
		}
        [HttpDelete]
        [Route("DeletePatientProfilePhoto")]
        [Authorize("default-policy", Roles = $"{Roles.Doctor}, {Roles.Admin}, {Roles.Patient}")]
        public async Task<IActionResult> PatchPatientProfilePhoto(long patientId)
        {
            BaseResponse<string> serviceResp = await _storageService.DeletePatientProfilePic(patientId);
            return Ok(serviceResp);
        }
    }
}
