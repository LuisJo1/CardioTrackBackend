using CardioTrackAPI.Data;
using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos.Treatment;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.InteropServices;

namespace CardioTrackAPI.Services
{
    public class TreatmentService : ITreatmentService
    {
        private readonly DBContext _dBContext;

        public TreatmentService(DBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public async Task<BaseResponse<string>> AddTreatmentAsync(AddTreatmentDto addTreatmentDto)
        {
			try
			{
				if(addTreatmentDto.DurationParameter.ToLower() != "week" && addTreatmentDto.DurationParameter.ToLower() != "day")
				{
					return BaseResponse<string>.GetError("The duration parameter can only be week or day", HttpStatusCode.BadRequest);
				}
				DateTime endDate = DateTime.Now;
				if(addTreatmentDto.DurationParameter == "week")
				{
					endDate = endDate.AddDays(addTreatmentDto.Duration / 7);
				} else if(addTreatmentDto.DurationParameter == "day")
				{
					endDate = endDate.AddDays(addTreatmentDto.Duration);
				}

				Treatment newTreatmentToAdd = new Treatment()
				{
					AdditionTime = DateTime.Now,
					Description = addTreatmentDto.Description,
					Duration = addTreatmentDto.Duration,
					DurationParameter = addTreatmentDto.DurationParameter,
					ExamId = addTreatmentDto.ExamId,
					TreatmentStartDate = DateTime.Now,
					TreatmentEndDate = endDate,
					PatientId = addTreatmentDto.PatientId,
				};
				await _dBContext.Treatment.AddAsync(newTreatmentToAdd);
				await _dBContext.SaveChangesAsync();

				List<TreatmentMedicines> treatmentMedicines = addTreatmentDto.TreatmentMedicines.Select(tmd => new TreatmentMedicines()
				{
					Duration = tmd.Duration,
					DurationParatemeter = tmd.DurationParameter,
					TreatmentId = newTreatmentToAdd.Id,
					MedicineName = tmd.MedicineName,
					TakeEvery = tmd.TakeEvery
				}).ToList();

				await _dBContext.TreatmentMedicine.AddRangeAsync(treatmentMedicines);
				await _dBContext.SaveChangesAsync();
				return BaseResponse<string>.GetSuccess("Ok", "created", HttpStatusCode.Created);
			}
			catch (Exception ex)
			{
				return BaseResponse<string>.GetError(ex.Message, HttpStatusCode.InternalServerError);
			}
        }

        public async Task<BaseResponse<SearchWithFilters<TreatmentDto>>> GetTreatmentsWithFilters(int sliceIndex, int sliceSize, TreatmentSearchFilters searchFilters)
        {
			try
			{
				IQueryable<Treatment> treatmentsQuery = _dBContext.Treatment
					.OrderByDescending(t => t.AdditionTime)
					.Include(t => t.treatmentMedicines)
					.Include(t => t.Exam)
					.ThenInclude(e => e!.Doctor);

				if(searchFilters.ExamId != 0 && searchFilters.ExamId != null)
				{
					treatmentsQuery = treatmentsQuery.Where(t => t.ExamId == searchFilters.ExamId);
				}
                if (searchFilters.PatientId != 0 && searchFilters.PatientId != null)
                {
                    treatmentsQuery = treatmentsQuery.Where(t => t.PatientId == searchFilters.PatientId);
                }
				if(searchFilters.GetLatest)
				{
					treatmentsQuery = treatmentsQuery.OrderByDescending(t => t.AdditionTime);
				}
                if (searchFilters.TreatmentId != 0 && searchFilters.TreatmentId != null)
                {
                    treatmentsQuery = treatmentsQuery.Where(t => t.Id == searchFilters.TreatmentId);
                }

                int searchResults = await treatmentsQuery.CountAsync();

				if(!searchFilters.GetLatest && !searchFilters.GetAll)
				{
						treatmentsQuery = treatmentsQuery.Skip((sliceIndex - 1) * sliceSize)
					.Take(sliceSize);
				} else if(!searchFilters.GetAll && searchFilters.GetLatest)
				{
                    treatmentsQuery = treatmentsQuery.Take(1);
                }

				List<TreatmentDto> treatmentDtos = await treatmentsQuery.Select(t => new TreatmentDto()
				{
					Id = t.Id,
					AdditionTime = t.AdditionTime,
					Description = t.Description,
					Duration = t.Duration,
					DurationParameter = t.DurationParameter,
					ExamId = t.ExamId,
					PatientId = t.PatientId,
					DoctorName = $"{t.Exam!.Doctor!.Names} {t.Exam.Doctor.Surnames}",
					TreatmentEndDate = t.TreatmentEndDate,
					TreatmentStartDate = t.TreatmentStartDate,
					TreatmentMedicines = t.treatmentMedicines!.Select(tm => new TreatmentMedicineDto()
					{
						Duration = tm.Duration,
						DurationParameter = tm.DurationParatemeter,
						Id = tm.Id,
						MedicineName = tm.MedicineName.Substring(0,1).ToUpper() + tm.MedicineName.Substring(1),
						TakeEvery = tm.TakeEvery,
						TreatmentId = tm.TreatmentId,
					}).ToList() ?? null
				}).ToListAsync();
				return BaseResponse<SearchWithFilters<TreatmentDto>>.GetSuccess("Ok",
					new SearchWithFilters<TreatmentDto>(treatmentDtos, searchResults), HttpStatusCode.OK);
            }
			catch (Exception ex)
			{
				return BaseResponse<SearchWithFilters<TreatmentDto>>.GetError(ex.Message, HttpStatusCode.InternalServerError);
			}
        }

        public Task<BaseResponse<string>> UpdateTreatmentMedicines(List<TreatmentMedicineDto> treatmentMedicineDtos)
        {
            throw new NotImplementedException();
        }
    }
}
