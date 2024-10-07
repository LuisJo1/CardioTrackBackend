using CardioTrackAPI.Data;
using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos.Patient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;

namespace CardioTrackAPI.Services
{
	public class PatientService : IPatientService
	{
		private readonly DBContext _dBContext;

		public PatientService(DBContext dBContext)
		{
			_dBContext = dBContext;
		}
		public async Task<BaseResponse<string>> AddPatientAsync(AddPatientDto addPatientRequest)
		{
			try
			{
				if (string.IsNullOrEmpty(addPatientRequest.Email))
				{
					throw new ArgumentException("Email is required");
				}
				else
				{
					bool isEmailUnique = await _dBContext.User.Where(user => user.Email!.ToUpper() == addPatientRequest.Email.ToUpper())
							.CountAsync() == 0;

					if (!isEmailUnique)
						throw new ArgumentException("That email is already being in use");
				}
				if (string.IsNullOrEmpty(addPatientRequest.Password))
				{
					throw new ArgumentException("Password is required");
				}
				else
				{
					if (addPatientRequest.Password.Length < 6)
					{
						throw new ArgumentException("Password must be more or equal to 6 characters");
					}
					if (addPatientRequest.Password.Length > 24)
					{
						throw new ArgumentException("Password must be minor than 24 characters");
					}
					if (addPatientRequest.ConfirmPassword != addPatientRequest.Password)
					{
						throw new ArgumentException("Password do not coincide");
					}
				}
				if (string.IsNullOrEmpty(addPatientRequest.CI))
				{
					throw new ArgumentException("CI is required");
				}
				if (string.IsNullOrEmpty(addPatientRequest.Names))
				{
					throw new ArgumentException("Names are required");
				}
				if (string.IsNullOrEmpty(addPatientRequest.Surnames))
				{
					throw new ArgumentException("Surnames are required");
				}
				if (string.IsNullOrEmpty(addPatientRequest.BornDate))
				{
					throw new ArgumentException("Borndate is required");
				}
				if (addPatientRequest.Genre != "M" && addPatientRequest.Genre != "F")
				{
					throw new ArgumentException("Genre is invalid");
				}
				
				User userToAdd = new User()
				{
					Email = addPatientRequest.Email,
					Password = BCrypt.Net.BCrypt.HashPassword(addPatientRequest.Password),
					RolId = int.Parse(Roles.Patient)
				};

				_dBContext.User.Add(userToAdd);
				await _dBContext.SaveChangesAsync();

				Patient PatientToAdd = new Patient()
				{
					BornDate = DateTime.ParseExact(addPatientRequest.BornDate, "yyyy/MM/dd", CultureInfo.InvariantCulture),
					CI = addPatientRequest.CI,
					Names = addPatientRequest.Names,
					Surnames = addPatientRequest.Surnames,
					UserId = userToAdd.Id,
					Genre = char.Parse(addPatientRequest.Genre)
				};

				_dBContext.Patient.Add(PatientToAdd);
				await _dBContext.SaveChangesAsync();

				return BaseResponse<string>.GetSuccess("Ok", "Created", HttpStatusCode.Created);
			}
			catch (ArgumentException ex)
			{
				return BaseResponse<string>.GetError(ex.Message, HttpStatusCode.BadRequest);
			}
			catch (Exception ex)
			{
				return BaseResponse<string>.GetError(ex.Message, HttpStatusCode.InternalServerError);
			}
		}

		public async Task<BaseResponse<string>> DeletePatientAsync(long PatientId)
		{
			try
			{
				Patient? PatientToRemove = await _dBContext.Patient.FindAsync(PatientId);
				if (PatientToRemove is null)
				{
					return BaseResponse<string>.GetError("Couldn't find a Patient to remove", HttpStatusCode.NotFound);
				}

				User? userToRemove = await _dBContext.User.FindAsync(PatientToRemove.UserId);

				if (userToRemove is null)
				{
					return BaseResponse<string>.GetError("Couldn't find the user asociated to Patient to remove", HttpStatusCode.NotFound);
				}


				_dBContext.Remove(PatientToRemove);
				_dBContext.Remove(userToRemove);
				await _dBContext.SaveChangesAsync();
				return BaseResponse<string>.GetSuccess("Ok", "Deleted", HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				return BaseResponse<string>.GetError(ex.Message, HttpStatusCode.InternalServerError);
			}
		}

		public async Task<BaseResponse<PatientDto>> GetPatientAsync(long PatientId)
		{
			try
			{
				Patient? patient = await _dBContext.Patient
					.Include(patient => patient.User)
					.Where(patient => patient.Id == PatientId)
					.FirstOrDefaultAsync();
				if (patient is null)
				{
					return BaseResponse<PatientDto>.GetError("Couldn't find a Patient with the given id", HttpStatusCode.NotFound);
				}
					int patientAge = DateTime.Now.Year - patient.BornDate.Year;
				if (DateTime.Now.Month < patient.BornDate.Month ||
				(DateTime.Now.Month == patient.BornDate.Month && DateTime.Now.Day < patient.BornDate.Day)) patientAge--;

				PatientDto PatientDto = new PatientDto
				{
					BornDate = patient.BornDate,
					CI = patient.CI,
					Email = patient.User?.Email ?? "",
					Names = patient.Names,
					Surnames = patient.Surnames,
					Genre = patient.Genre,
					Age = patientAge
				};
				return BaseResponse<PatientDto>.GetSuccess("Ok", PatientDto, HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				return BaseResponse<PatientDto>.GetError(ex.Message, HttpStatusCode.InternalServerError);
			}
		}

		public async Task<BaseResponse<SearchWithFilters<PatientDto>>> GetPatientsWithFilters(int sliceIndex, int sliceSize, PatientSearchFilters filters)
		{
			try
			{
				IQueryable<Patient> patientsQuery = _dBContext.Patient
					.Include(patient => patient.User);

				if(!string.IsNullOrEmpty(filters.CI))
				{
					patientsQuery = patientsQuery.Where(patient => patient.CI == filters.CI);
				}
				if(!string.IsNullOrEmpty(filters.FullName))
				{
					string trimmedFullName = filters.FullName.Replace(" ", string.Empty);
					patientsQuery = patientsQuery.Where(patient => patient.Names.Replace(" ", string.Empty).ToLower()+patient.Surnames.Replace(" ", string.Empty).ToLower() == trimmedFullName.ToLower());
				}
				int searchResults = await patientsQuery.CountAsync();

				patientsQuery = patientsQuery.Skip((sliceIndex - 1) * sliceSize)
				.Take(sliceSize);

				List<PatientDto> result = new List<PatientDto>();

				await foreach(Patient patient in patientsQuery.AsAsyncEnumerable())
				{
					int patientAge = DateTime.Now.Year - patient.BornDate.Year;
					if (DateTime.Now.Month < patient.BornDate.Month ||
					(DateTime.Now.Month == patient.BornDate.Month && DateTime.Now.Day < patient.BornDate.Day)) patientAge--;
					PatientDto patientDto = new PatientDto()
					{
						Age = patientAge,
						BornDate = patient.BornDate,
						CI = patient.CI,
						Email = patient.User?.Email ?? "",
						Names = patient.Names,
						Surnames = patient.Surnames,
						Genre = patient.Genre
					};
					result.Add(patientDto);
				}
				return BaseResponse<SearchWithFilters<PatientDto>>.GetSuccess("Ok", new SearchWithFilters<PatientDto>(result, searchResults), HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				return BaseResponse<SearchWithFilters<PatientDto>>.GetError(ex.Message, HttpStatusCode.InternalServerError);
			}
		}

		public async Task<BaseResponse<string>> PatchPatientAsync(long PatientId, PatchPatientDto patchPatientRequest)
		{
			try
			{
				Patient? PatientToPatch = await _dBContext.Patient
				.Include(Patient => Patient.User)
				.Where(Patient => Patient.Id == PatientId)
				.FirstOrDefaultAsync();

				if (PatientToPatch is null)
				{
					return BaseResponse<string>.GetError("Couldn't find a Patient with the given id", HttpStatusCode.NotFound);
				}
				if (!string.IsNullOrEmpty(patchPatientRequest.Email))
				{
					bool isEmailUnique = await _dBContext.User.Where(user => user.Email!.ToUpper() == patchPatientRequest.Email.ToUpper())
						.CountAsync() == 0;

					if (!isEmailUnique)
						throw new ArgumentException("That email is already being in use");

					PatientToPatch.User!.Email = patchPatientRequest.Email;
				}
				if (!string.IsNullOrEmpty(patchPatientRequest.Password))
				{
					if (patchPatientRequest.Password.Length < 6)
					{
						throw new ArgumentException("Password must be more or equal to 6 characters");
					}
					if (patchPatientRequest.Password.Length > 24)
					{
						throw new ArgumentException("Password must be minor than 24 characters");
					}
					
					PatientToPatch.User!.Password = BCrypt.Net.BCrypt.HashPassword(patchPatientRequest.Password);
				}
				if (!string.IsNullOrEmpty(patchPatientRequest.Names))
				{
					PatientToPatch.Names = patchPatientRequest.Names;
				}
				if (!string.IsNullOrEmpty(patchPatientRequest.Surnames))
				{
					PatientToPatch.Surnames = patchPatientRequest.Surnames;
				}
				if (!string.IsNullOrEmpty(patchPatientRequest.CI))
				{
					PatientToPatch.CI = patchPatientRequest.CI;
				}
				if (!string.IsNullOrEmpty(patchPatientRequest.BornDate))
				{
					PatientToPatch.BornDate = DateTime.ParseExact(patchPatientRequest.BornDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
				}
				if(!string.IsNullOrEmpty(patchPatientRequest.Genre))
				{
					if (patchPatientRequest.Genre != "M" && patchPatientRequest.Genre != "F")
					{
						throw new ArgumentException("Genre is invalid");
					}
					PatientToPatch.Genre = char.Parse(patchPatientRequest.Genre);
				}
				await _dBContext.SaveChangesAsync();
				return BaseResponse<string>.GetSuccess("Ok", "patched", HttpStatusCode.OK);
			}
			catch (ArgumentException ex)
			{
				return BaseResponse<string>.GetError(ex.Message, HttpStatusCode.BadRequest);
			}
			catch (Exception ex)
			{
				return BaseResponse<string>.GetError(ex.Message, HttpStatusCode.InternalServerError);
			}
		}

		public async Task<BaseResponse<string>> UpdatePatientAsync(long PatientId, UpdatePatientDto updatePatientRequest)
		{
			try
			{
				if (string.IsNullOrEmpty(updatePatientRequest.Email))
				{
					throw new ArgumentException("Email is required");
				}
				else
				{
					bool isEmailUnique = await _dBContext.User.Where(user => user.Email!.ToUpper() == updatePatientRequest.Email.ToUpper())
					.CountAsync() == 0;

					if (!isEmailUnique)
						throw new ArgumentException("That email is already being in use");

				}
				if (string.IsNullOrEmpty(updatePatientRequest.Password))
				{
					throw new ArgumentException("Password is required");
				}
				else
				{
					if (updatePatientRequest.Password.Length < 6)
					{
						throw new ArgumentException("Password must be more or equal to 6 characters");
					}
					if (updatePatientRequest.Password.Length > 24)
					{
						throw new ArgumentException("Password must be minor than 24 characters");
					}
				}
				if (string.IsNullOrEmpty(updatePatientRequest.CI))
				{
					throw new ArgumentException("CI is required");
				}
				if (string.IsNullOrEmpty(updatePatientRequest.Names))
				{
					throw new ArgumentException("Names are required");
				}
				if (string.IsNullOrEmpty(updatePatientRequest.Surnames))
				{
					throw new ArgumentException("Surnames are required");
				}
				if (string.IsNullOrEmpty(updatePatientRequest.BornDate))
				{
					throw new ArgumentException("Borndate is required");
				}
				if (updatePatientRequest.Genre != "M" && updatePatientRequest.Genre != "F")
				{
					throw new ArgumentException("Genre is invalid");
				}

				Patient? PatientToEdit = await _dBContext.Patient
				.Include(Patient => Patient.User)
				.Where(Patient => Patient.Id == PatientId)
				.FirstOrDefaultAsync();

				if (PatientToEdit is null)
				{
					return BaseResponse<string>.GetError("Couldn't find a Patient with the given id", HttpStatusCode.NotFound);
				}

				
				PatientToEdit.BornDate = DateTime.ParseExact(updatePatientRequest.BornDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
				PatientToEdit.Names = updatePatientRequest.Names;
				PatientToEdit.Surnames = updatePatientRequest.Surnames;
				PatientToEdit.CI = updatePatientRequest.CI;
				PatientToEdit.User!.Email = updatePatientRequest.Email;
				PatientToEdit.User!.Password = BCrypt.Net.BCrypt.HashPassword(updatePatientRequest.Password);
				PatientToEdit.Genre = char.Parse(updatePatientRequest.Genre);

				await _dBContext.SaveChangesAsync();
				return BaseResponse<string>.GetSuccess("Ok", "Edited", HttpStatusCode.OK);
			}
			catch (ArgumentException ex)
			{
				return BaseResponse<string>.GetError(ex.Message, HttpStatusCode.BadRequest);
			}
			catch (Exception ex)
			{
				return BaseResponse<string>.GetError(ex.Message, HttpStatusCode.InternalServerError);
			}
		}
	}
}
