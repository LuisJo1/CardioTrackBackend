using CardioTrackAPI.Data;
using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Security.Cryptography;
using System.Text;

namespace CardioTrackAPI.Services
{
	public class ForgotPasswordService : IForgotPasswordService
	{
		private readonly DBContext _dBContext;

		public ForgotPasswordService(DBContext dBContext)
		{
			_dBContext = dBContext;
		}

		public async Task<BaseResponse<string>> GetRecoverTokenAsync(string userEmail)
		{
			try
			{
				User? userRequesting = await _dBContext.User.Where(user => user.Email.ToLower() == userEmail.ToLower())
					.FirstOrDefaultAsync();

				if (userRequesting is null) 
					return BaseResponse<string>.GetError("Sorry, there is no user with the given email", System.Net.HttpStatusCode.BadRequest);

				if (userRequesting.LastPasswordRecoveryTimeUTC != null && DateTime.UtcNow - userRequesting.LastPasswordRecoveryTimeUTC < TimeSpan.FromHours(1))
					return BaseResponse<string>.GetError("You can only request a password recovery link once every 5 hours.", System.Net.HttpStatusCode.BadRequest);

				BaseResponse<bool> alreadyHasAPendingToken = await HasPendingTokenAsync(userRequesting.Id);
				if (!alreadyHasAPendingToken.Success) return BaseResponse<string>.GetError(alreadyHasAPendingToken.Message, System.Net.HttpStatusCode.InternalServerError);

				if (alreadyHasAPendingToken.Data == true)
					throw new Exception(alreadyHasAPendingToken.Message);


				string recoveryToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
				string? userName = null;
				Doctor? doctorAssociated = await _dBContext.Doctor.Where(doctor => doctor.UserId == userRequesting.Id)
					.FirstOrDefaultAsync();

				Patient? patientAssociated = null;
				if (doctorAssociated is null)
				{
					patientAssociated = await _dBContext.Patient.Where(patient => patient.UserId == userRequesting.Id)
						.FirstOrDefaultAsync();
				}
				if (patientAssociated is not null)
				{
					userName = patientAssociated.Names + " " + patientAssociated.Surnames;
				}
				else if (doctorAssociated is not null)
				{
					userName = doctorAssociated.Names + " " + doctorAssociated.Surnames;
				}
				PasswordRecoverToken passwordRecoverToken = new PasswordRecoverToken()
				{
					CreationTimeUTC = DateTime.UtcNow,
					Revoked = false,
					Token = recoveryToken.ToSHA256(),
					UserId = userRequesting.Id
				};

				_dBContext.PasswordRecoverToken.Add(passwordRecoverToken);
				await _dBContext.SaveChangesAsync();

				MimeMessage email = new MimeMessage();

				email.From.Add(new MailboxAddress("CardioTrack", "cardiotrackofficial@gmail.com"));
				email.To.Add(new MailboxAddress("customer", userEmail));

				email.Subject = "Reestablecer su contraseña";

				email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
				{
					Text = $"<h2>Hola, {userName ?? "admin"}</h2></br><div style='font-size:16px;'>La clave para reestablecer su contraseña es <span style='color: #00f'>{recoveryToken}</span></div>"
				};

				using (MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient())
				{
					smtpClient.Connect("smtp.gmail.com", 587, false);
					await smtpClient.AuthenticateAsync("cardiotrackofficial@gmail.com", "azgf sgxm lbnj leno");

					smtpClient.Send(email);
					smtpClient.Disconnect(true);
				}
				return BaseResponse<string>.GetSuccess("Ok", "Email sent", System.Net.HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				return BaseResponse<string>.GetError(ex.Message, System.Net.HttpStatusCode.InternalServerError);
			}
		}

		public async Task<BaseResponse<bool>> HasPendingTokenAsync(long userId)
		{
			try
			{
				List<PasswordRecoverToken> passwordRecoverTokensToClear = await _dBContext.PasswordRecoverToken.Where(passwordRecoverToken => passwordRecoverToken.UserId == userId)
					.ToListAsync();
				_dBContext.RemoveRange(passwordRecoverTokensToClear.Where(p => (DateTime.UtcNow - p.CreationTimeUTC).Minutes > 1));
				await _dBContext.SaveChangesAsync();

				int count = await _dBContext.PasswordRecoverToken.Where(passwordRecoverToken => passwordRecoverToken.UserId == userId)
					.CountAsync();

				if (count > 0) return BaseResponse<bool>.GetSuccess("You already have a pending token", true);
				return BaseResponse<bool>.GetSuccess("User don't have any pending token", false);
			}
			catch (Exception ex)
			{
				return BaseResponse<bool>.GetError(ex.Message, System.Net.HttpStatusCode.InternalServerError);
			}
		}

		public async Task<BaseResponse<string>> RecoverUserPasswordAsync(RecoverPasswordDto recoverPasswordRequestDto)
		{
			try
			{
				User? userRequesting = await _dBContext.User.Where(user => user.Email.ToLower() == recoverPasswordRequestDto.UserEmail.ToLower())
					.FirstOrDefaultAsync();
				if (userRequesting is null) throw new Exception("Sorry, there is no user with the given email");

				List<PasswordRecoverToken> validUserTokens = await _dBContext.PasswordRecoverToken
					.Where(passwordRecoverToken => passwordRecoverToken.UserId == userRequesting.Id)
					.ToListAsync();

				validUserTokens = validUserTokens.Where(p => (DateTime.UtcNow - p.CreationTimeUTC).Minutes <= 5).ToList();

				if (validUserTokens.Count == 0)
					return BaseResponse<string>.GetError("Token has expired. Please request a new recovery link.", System.Net.HttpStatusCode.BadRequest);
				string hashedTokenFromUserReq = recoverPasswordRequestDto.Token.ToSHA256();
				if (validUserTokens.Where(v => v.Token == hashedTokenFromUserReq).Count() == 0)
					return BaseResponse<string>.GetError("Token has expired. Please request a new recovery link.", System.Net.HttpStatusCode.BadRequest);

				if (string.IsNullOrEmpty(recoverPasswordRequestDto.NewPassword))
				{
					throw new ArgumentException("Password is required");
				}
				else
				{
					if (recoverPasswordRequestDto.NewPassword.Length < 6)
					{
						throw new ArgumentException("Password must be more or equal to 6 characters");
					}
					if (recoverPasswordRequestDto.NewPassword.Length > 24)
					{
						throw new ArgumentException("Password must be minor than 24 characters");
					}
					if (recoverPasswordRequestDto.ConfirmPassword != recoverPasswordRequestDto.NewPassword)
					{
						throw new ArgumentException("Password don't coincide");
					}
				}

				userRequesting.Password = BCrypt.Net.BCrypt.HashPassword(recoverPasswordRequestDto.NewPassword);
				await _dBContext.SaveChangesAsync();

				_dBContext.RemoveRange(validUserTokens);
				await _dBContext.SaveChangesAsync();

				userRequesting.LastPasswordRecoveryTimeUTC = DateTime.UtcNow;
				await _dBContext.SaveChangesAsync();

				return BaseResponse<string>.GetSuccess("Ok", "Password recovered", System.Net.HttpStatusCode.OK);
			}
			catch (ArgumentException ex)
			{
				return BaseResponse<string>.GetError(ex.Message, System.Net.HttpStatusCode.BadRequest);
			}
			catch (Exception ex)
			{
				return BaseResponse<string>.GetError(ex.Message, System.Net.HttpStatusCode.InternalServerError);
			}
		}
	}

	public static class StringExtensions
	{
		public static string ToSHA256(this string value)
		{
			using var sha = SHA256.Create();

			var bytes = Encoding.UTF8.GetBytes(value);
			var hash = sha.ComputeHash(bytes);

			return Convert.ToBase64String(hash);
		}
	}

}
