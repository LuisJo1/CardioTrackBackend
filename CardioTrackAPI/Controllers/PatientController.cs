using CardioTrackAPI.Model.Dtos.Patient;
using CardioTrackAPI.Model;
using CardioTrackAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrackAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class PatientController : ControllerBase
	{
		private readonly IPatientService _patientService;

		public PatientController(IPatientService patientService)
		{
			_patientService = patientService;
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
		[Authorize("default-policy", Roles = $"{Roles.Patient}, {Roles.Admin}")]
		public async Task<ActionResult<BaseResponse<string>>> UpdatePatient([FromQuery] long patientId, [FromBody] UpdatePatientDto updatePatientRequest)
		{
			BaseResponse<string> serviceResp = await _patientService.UpdatePatientAsync(patientId, updatePatientRequest);
			return Ok(serviceResp);
		}

		[HttpPatch]
		[Route("PatchPatient")]
		[Authorize("default-policy", Roles = $"{Roles.Patient}, {Roles.Admin}")]
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
		[Route("GetPatientWithFilters")]
		public async Task<IActionResult> GetPatientWithFilters([FromQuery]int sliceIndex = 1, [FromQuery]int sliceSize = 10, string? ci = "", string? fullName = "")
		{
			BaseResponse<SearchWithFilters<PatientDto>> serviceResp = await _patientService.GetPatientsWithFilters(sliceIndex, sliceSize, new PatientSearchFilters
			{
				CI = ci,
				FullName = fullName
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
	}
}
