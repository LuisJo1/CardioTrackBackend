using CardioTrackAPI.Data;
using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos.Doctor;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net;

namespace CardioTrackAPI.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly DBContext _dBContext;

        public DoctorService(DBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public async Task<BaseResponse<string>> AddDoctorAsync(AddDoctorDto addDoctorRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(addDoctorRequest.Email))
                {
                    throw new ArgumentException("Email is required");
                }
                else
                {
                    bool isEmailUnique = await _dBContext.User.Where(user => user.Email!.ToUpper() == addDoctorRequest.Email.ToUpper())
                            .CountAsync() == 0;

                    if (!isEmailUnique)
                        throw new ArgumentException("That email is already being in use");
                }
                if (string.IsNullOrEmpty(addDoctorRequest.Password))
                {
                    throw new ArgumentException("Password is required");
                }
                else
                {
                    if (addDoctorRequest.Password.Length < 6)
                    {
                        throw new ArgumentException("Password must be more or equal to 6 characters");
                    }
                    if (addDoctorRequest.Password.Length > 24)
                    {
                        throw new ArgumentException("Password must be minor than 24 characters");
                    }
                    if (addDoctorRequest.ConfirmPassword != addDoctorRequest.Password)
                    {
                        throw new ArgumentException("Password do not coincide");
                    }
                }
                if (string.IsNullOrEmpty(addDoctorRequest.CI))
                {
                    throw new ArgumentException("CI is required");
                }
                if (string.IsNullOrEmpty(addDoctorRequest.Names))
                {
                    throw new ArgumentException("Names are required");
                }
                if (string.IsNullOrEmpty(addDoctorRequest.Surnames))
                {
                    throw new ArgumentException("Surnames are required");
                }
                if (string.IsNullOrEmpty(addDoctorRequest.BornDate))
                {
                    throw new ArgumentException("Borndate is required");
                }
                if (addDoctorRequest.Genre != "M" && addDoctorRequest.Genre != "F")
                {
                    throw new ArgumentException("Genre is invalid");
                }

                User userToAdd = new User()
                {
                    Email = addDoctorRequest.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(addDoctorRequest.Password),
                    RolId = int.Parse(Roles.Doctor)
                };

                _dBContext.User.Add(userToAdd);
                await _dBContext.SaveChangesAsync();

                Doctor doctorToAdd = new Doctor()
                {
                    BornDate = DateTime.ParseExact(addDoctorRequest.BornDate, "yyyy/MM/dd", CultureInfo.InvariantCulture),
                    CI = addDoctorRequest.CI,
                    Names = addDoctorRequest.Names,
                    Surnames = addDoctorRequest.Surnames,
                    UserId = userToAdd.Id,
                    Specialty = addDoctorRequest.Specialty,
                    Genre = char.Parse(addDoctorRequest.Genre),
                    PhoneNumber = addDoctorRequest.PhoneNumber
                };

                _dBContext.Doctor.Add(doctorToAdd);
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

        public async Task<BaseResponse<string>> DeleteDoctorAsync(long doctorId)
        {
            try
            {
                Doctor? doctorToRemove = await _dBContext.Doctor.FindAsync(doctorId);
                if (doctorToRemove is null)
                {
                    return BaseResponse<string>.GetError("Couldn't find a doctor to remove", HttpStatusCode.NotFound);
                }

                User? userToRemove = await _dBContext.User.FindAsync(doctorToRemove.UserId);

                if (userToRemove is null)
                {
                    return BaseResponse<string>.GetError("Couldn't find the user asociated to doctor to remove", HttpStatusCode.NotFound);
                }


                _dBContext.Remove(doctorToRemove);
                _dBContext.Remove(userToRemove);
                await _dBContext.SaveChangesAsync();
                return BaseResponse<string>.GetSuccess("Ok", "Deleted", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.GetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<BaseResponse<List<DoctorDto>>> GetAllDoctorsAsync()
        {
            try
            {
                List<Doctor> doctors = await _dBContext.Doctor
                    .Include(d => d.User)
                    .ToListAsync();
                List<DoctorDto> doctorsDtos = doctors.Select(d => new DoctorDto
                {
                    BornDate = d.BornDate,
                    CI = d.CI,
                    Email = d.User?.Email ?? "",
                    Genre = d.Genre.ToString(),
                    Id = d.Id,
                    Names = d.Names,
                    Surnames = d.Surnames,
                    PhoneNumber = d.PhoneNumber,
                    Specialty = d.Specialty,
                }).ToList();

                return BaseResponse<List<DoctorDto>>.GetSuccess("Doctors retrieved successfully", doctorsDtos, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<DoctorDto>>.GetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<BaseResponse<DoctorDto>> GetDoctorAsync(long doctorId)
        {
            try
            {
                Doctor? doctor = await _dBContext.Doctor
                    .Include(doctor => doctor.User)
                    .Where(doctor => doctor.Id == doctorId)
                    .FirstOrDefaultAsync();
                if (doctor is null)
                {
                    return BaseResponse<DoctorDto>.GetError("Couldn't find a doctor with the given id", HttpStatusCode.NotFound);
                }
                DoctorDto doctorDto = new DoctorDto
                {
                    Id = doctor.Id,
                    BornDate = doctor.BornDate,
                    CI = doctor.CI,
                    Email = doctor.User?.Email ?? "",
                    Names = doctor.Names,
                    Surnames = doctor.Surnames,
                    Specialty = doctor.Specialty,
                    Genre = doctor.Genre.ToString(),
                    PhoneNumber = doctor.PhoneNumber
                };
                return BaseResponse<DoctorDto>.GetSuccess("Ok", doctorDto, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return BaseResponse<DoctorDto>.GetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<BaseResponse<DoctorDto>> GetDoctorByUserIdAsync(long userId)
        {
            try
            {
                Doctor? doctor = await _dBContext.Doctor
                    .Include(doctor => doctor.User)
                    .Where(doctor => doctor.UserId == userId)
                    .FirstOrDefaultAsync();
                if (doctor is null)
                {
                    return BaseResponse<DoctorDto>.GetError("Couldn't find a doctor with the given id", HttpStatusCode.NotFound);
                }
                DoctorDto doctorDto = new DoctorDto
                {
                    Id = doctor.Id,
                    BornDate = doctor.BornDate,
                    CI = doctor.CI,
                    Email = doctor.User?.Email ?? "",
                    Names = doctor.Names,
                    Surnames = doctor.Surnames,
                    Specialty = doctor.Specialty,
                    Genre = doctor.Genre.ToString(),
                    PhoneNumber = doctor.PhoneNumber
                };
                return BaseResponse<DoctorDto>.GetSuccess("Ok", doctorDto, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return BaseResponse<DoctorDto>.GetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<BaseResponse<string>> PatchDoctorAsync(long doctorId, PatchDoctorDto patchDoctorRequest)
        {
            try
            {
                Doctor? doctorToPatch = await _dBContext.Doctor
                .Include(doctor => doctor.User)
                .Where(doctor => doctor.Id == doctorId)
                .FirstOrDefaultAsync();

                if (doctorToPatch is null)
                {
                    return BaseResponse<string>.GetError("Couldn't find a doctor with the given id", HttpStatusCode.NotFound);
                }
                if (!string.IsNullOrEmpty(patchDoctorRequest.Email))
                {
                    bool isEmailUnique = await _dBContext.User.Where(user => user.Email!.ToUpper() == patchDoctorRequest.Email.ToUpper())
                        .CountAsync() == 0;

                    if (!isEmailUnique)
                        throw new ArgumentException("That email is already being in use");

                    doctorToPatch.User!.Email = patchDoctorRequest.Email;
                }
                if (!string.IsNullOrEmpty(patchDoctorRequest.Password))
                {

                    if (patchDoctorRequest.Password.Length < 6)
                    {
                        throw new ArgumentException("Password must be more or equal to 6 characters");
                    }
                    if (patchDoctorRequest.Password.Length > 24)
                    {
                        throw new ArgumentException("Password must be minor than 24 characters");
                    }
                    doctorToPatch.User!.Password = BCrypt.Net.BCrypt.HashPassword(patchDoctorRequest.Password);
                }
                if (!string.IsNullOrEmpty(patchDoctorRequest.Names))
                {
                    doctorToPatch.Names = patchDoctorRequest.Names;
                }
                if (!string.IsNullOrEmpty(patchDoctorRequest.Surnames))
                {
                    doctorToPatch.Surnames = patchDoctorRequest.Surnames;
                }
                if (!string.IsNullOrEmpty(patchDoctorRequest.CI))
                {
                    doctorToPatch.CI = patchDoctorRequest.CI;
                }
                if (!string.IsNullOrEmpty(patchDoctorRequest.BornDate))
                {
                    doctorToPatch.BornDate = DateTime.ParseExact(patchDoctorRequest.BornDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(patchDoctorRequest.Genre))
                {
                    if (patchDoctorRequest.Genre != "M" && patchDoctorRequest.Genre != "F")
                    {
                        throw new ArgumentException("Genre is invalid");
                    }
                    doctorToPatch.Genre = char.Parse(patchDoctorRequest.Genre);
                }
                if (!string.IsNullOrEmpty(patchDoctorRequest.PhoneNumber))
                {
                    doctorToPatch.PhoneNumber = patchDoctorRequest.PhoneNumber;
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

        public async Task<BaseResponse<string>> UpdateDoctorAsync(long doctorId, UpdateDoctorDto updateDoctorRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(updateDoctorRequest.Email))
                {
                    throw new ArgumentException("Email is required");
                }
                else
                {
                    bool isEmailUnique = await _dBContext.User.Where(user => user.Email!.ToUpper() == updateDoctorRequest.Email.ToUpper())
                    .CountAsync() == 0;

                    if (!isEmailUnique)
                        throw new ArgumentException("That email is already being in use");

                }
                if (string.IsNullOrEmpty(updateDoctorRequest.Password))
                {
                    throw new ArgumentException("Password is required");
                }
                else
                {
                    if (updateDoctorRequest.Password.Length < 6)
                    {
                        throw new ArgumentException("Password must be more or equal to 6 characters");
                    }
                    if (updateDoctorRequest.Password.Length > 24)
                    {
                        throw new ArgumentException("Password must be minor than 24 characters");
                    }
                }
                if (string.IsNullOrEmpty(updateDoctorRequest.CI))
                {
                    throw new ArgumentException("CI is required");
                }
                if (string.IsNullOrEmpty(updateDoctorRequest.Names))
                {
                    throw new ArgumentException("Names are required");
                }
                if (string.IsNullOrEmpty(updateDoctorRequest.Surnames))
                {
                    throw new ArgumentException("Surnames are required");
                }
                if (string.IsNullOrEmpty(updateDoctorRequest.BornDate))
                {
                    throw new ArgumentException("Borndate is required");
                }
                if (string.IsNullOrEmpty(updateDoctorRequest.Genre))
                {
                    throw new ArgumentException("Genre is required");
                }
                if (updateDoctorRequest.Genre != "M" && updateDoctorRequest.Genre != "F")
                {
                    throw new ArgumentException("Genre is invalid");
                }

                Doctor? doctorToEdit = await _dBContext.Doctor
                .Include(doctor => doctor.User)
                .Where(doctor => doctor.Id == doctorId)
                .FirstOrDefaultAsync();

                if (doctorToEdit is null)
                {
                    return BaseResponse<string>.GetError("Couldn't find a doctor with the given id", HttpStatusCode.NotFound);
                }

                doctorToEdit.BornDate = DateTime.ParseExact(updateDoctorRequest.BornDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                doctorToEdit.Names = updateDoctorRequest.Names;
                doctorToEdit.Surnames = updateDoctorRequest.Surnames;
                doctorToEdit.CI = updateDoctorRequest.CI;
                doctorToEdit.User!.Email = updateDoctorRequest.Email;
                doctorToEdit.User!.Password = BCrypt.Net.BCrypt.HashPassword(updateDoctorRequest.Password);
                doctorToEdit.Genre = char.Parse(updateDoctorRequest.Genre);
                if (!string.IsNullOrEmpty(updateDoctorRequest.PhoneNumber))
                {
                    doctorToEdit.PhoneNumber = updateDoctorRequest.PhoneNumber;
                }

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
