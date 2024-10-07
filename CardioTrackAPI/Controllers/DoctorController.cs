using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos.Doctor;
using CardioTrackAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrackAPI.Controllers
{
    [Route("[controller]")]
	[ApiController]
	public class DoctorController : ControllerBase
	{
		private readonly IDoctorService _doctorService;

		public DoctorController(IDoctorService doctorService)
		{
			_doctorService = doctorService;
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
	}
}
