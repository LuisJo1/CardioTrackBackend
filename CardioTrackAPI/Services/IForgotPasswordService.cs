using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos;

namespace CardioTrackAPI.Services
{
	public interface IForgotPasswordService
	{
		Task<BaseResponse<string>> GetRecoverTokenAsync(string userEmail);
		Task<BaseResponse<string>> RecoverUserPasswordAsync(RecoverPasswordDto recoverPasswordRequestDto);
	}
}
