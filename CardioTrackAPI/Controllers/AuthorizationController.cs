using CardioTrackAPI.Data;
using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos;
using CardioTrackAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace CardioTrackAPI.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthorizationController : ControllerBase
	{
		private readonly DBContext _dbContext;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IForgotPasswordService _forgotPasswordService;

		public AuthorizationController(DBContext dbContext, IHttpContextAccessor httpContextAccessor, IForgotPasswordService forgotPasswordService)
        {
			_dbContext = dbContext;
			_httpContextAccessor = httpContextAccessor;
			_forgotPasswordService = forgotPasswordService;
		}

        [HttpPost]
		[Route("Login")]
		public async Task<ActionResult<BaseResponse<string>>> Login(SignInRequest signInRequest)
		{
			try
			{
				User? user = await _dbContext.User.
					FirstOrDefaultAsync(user => user.Email!.ToLower() == signInRequest.Email.ToLower());

				if (user == null)
				{
					return BaseResponse<string>.GetError("There is no user registered with that email", HttpStatusCode.BadRequest);
				}
				if (!BCrypt.Net.BCrypt.Verify(signInRequest.Password, user.Password)) return BaseResponse<string>.GetError("Email or password incorrect", HttpStatusCode.Unauthorized);

				List<Claim> claims = new List<Claim>()
				{
					new Claim(ClaimTypes.NameIdentifier, user.Email!),
					new Claim(ClaimTypes.Sid, user.Id.ToString()),
					new Claim(ClaimTypes.Role, user.RolId.ToString()),
				};

				ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "cookie");

				ClaimsPrincipal User = new ClaimsPrincipal(claimsIdentity);

				await _httpContextAccessor.HttpContext!.SignInAsync("cookie", User, new AuthenticationProperties
				{
					IsPersistent = true
				});

				return Ok(BaseResponse<string>.GetSuccess("Ok", "", HttpStatusCode.OK));
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpGet]
		[Route("ValidateUser")]
		[Authorize("default-policy")]
		public async Task<ActionResult<BaseResponse<UserDataDto>>> ValidateUser([FromQuery]List<string>? roles)
		{
			if(roles != null && roles.Count > 0)
			{
				int count = 0;
				foreach(string rol in roles)
				{
					if (_httpContextAccessor.HttpContext!.User.IsInRole(rol)) count++;
				}
				if (count == 0) return BaseResponse<UserDataDto>.GetError("User doesn't meet the required permissions", HttpStatusCode.Forbidden);
			}
			User? user = await _dbContext.User
				.Include(user => user.Rol)
				.Where(user => user.Id == long.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Sid)!))
				.FirstOrDefaultAsync();

			if(user == null) return BaseResponse<UserDataDto>.GetError("User not found", HttpStatusCode.NotFound);

			UserDataDto userDataDto = new UserDataDto
			{
				Email = user.Email,
				Id = user.Id.ToString(),
				RolId = user.RolId,
				RolName = user.Rol!.Name
			};
			return Ok(BaseResponse<UserDataDto>.GetSuccess("Ok", userDataDto, HttpStatusCode.OK));
		}

		[HttpPost]
		[Route("Logout")]
		public async Task<ActionResult<BaseResponse<string>>> Logout()
		{
			try
			{
				await _httpContextAccessor.HttpContext!.SignOutAsync("cookie");
				return Ok(BaseResponse<string>.GetSuccess("Ok", "logout", HttpStatusCode.OK));
			}
			catch (Exception ex)
			{
				return new ObjectResult(BaseResponse<string>.GetError(ex.Message, HttpStatusCode.InternalServerError))
				{
					StatusCode = (int)HttpStatusCode.InternalServerError
				};
			}
		}

		[HttpPost]
		[Route("GetRecoverPasswordToken")]
		public async Task<IActionResult> ForgotPassword([FromQuery]string userEmail)
		{
			if (_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Sid) is not null) return Forbid();
			BaseResponse<string> serviceResp = await _forgotPasswordService.GetRecoverTokenAsync(userEmail);
			return Ok(serviceResp);
		}

		[HttpPatch]
		[Route("RecoverPassword")]
		public async Task<IActionResult> RecoverPassword([FromBody]RecoverPasswordDto recoverPasswordRequestDto)
		{
			if (_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Sid) is not null) return Forbid();
			BaseResponse<string> serviceResp = await _forgotPasswordService.RecoverUserPasswordAsync(recoverPasswordRequestDto);
			return Ok(serviceResp);
		}
	}
}
