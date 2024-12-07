using CardioTrackAPI.Data;
using CardioTrackAPI.Model;
using CardioTrackAPI.Model.Dtos.Exam;
using CardioTrackAPI.Model.Dtos.PersonalBackground;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CardioTrackAPI.Services
{
	public class ExamService : IExamService
	{
        private readonly DBContext _dBContext;

        public ExamService(DBContext dBContext)
        {
            _dBContext = dBContext;
        }
        public async Task<BaseResponse<string>> AddExamAsync(AddExamDto addExamDto, long userId)
		{
			try
			{
				Doctor? doctorRequesting = await _dBContext.Doctor.Where(d => d.UserId == userId).FirstOrDefaultAsync();
				if (doctorRequesting == null)
				{
					return BaseResponse<string>.GetError("Couldn't get doctor requesting", HttpStatusCode.NotFound);
				}
				Patient? patientBeingIssue = await _dBContext.Patient.Where(p => p.Id == addExamDto.PatientId).FirstOrDefaultAsync();
				if(patientBeingIssue == null)
				{
                    return BaseResponse<string>.GetError("Couldn't get the patient with the given ID", HttpStatusCode.NotFound);
                }

				Exam newExamToAdd = new Exam()
				{
					DoctorId = doctorRequesting.Id,
					EvaluationDate = DateTime.Now,
					PatientId = patientBeingIssue.Id,
					InterventionProposed = addExamDto.InterventionProposed,
				};

				await _dBContext.Exam.AddAsync(newExamToAdd);
				await _dBContext.SaveChangesAsync();

				var pb = addExamDto.AddPersonalBackgroundDto;
				PersonalBackground newPersonalBackgroundToAdd = new PersonalBackground()
				{
					ArterialHypertension = pb.ArterialHypertension,
					MyocardialInfarction = pb.MyocardialInfarction,
					Asthma = pb.Asthma,
					Pneumopathy = pb.Pneumopathy,
					Dyslipidemia = pb.Dyslipidemia,
					Angina = pb.Angina,
					LiverDisease = pb.LiverDisease,
					Homeopathy = pb.Homeopathy,
					Stroke = pb.Stroke,
					DiabetesMellitus = pb.DiabetesMellitus,
					Chagas = pb.Chagas,
					Thyroidopathy = pb.Thyroidopathy,
					OtherPersonalBackground = string.Join(", ", pb.OtherPersonalBackground!),
					Surgery = pb.Surgery,
					SurgeryType = pb.SurgeryType,
					GestationWeeks = pb.Obstetric!.GestationWeeks,
					For = pb.Obstetric!.For,
					Caesarean = pb.Obstetric.Caesarean,
					Stillbirth = pb.Obstetric.Stillbirth,
					Abortion = pb.Obstetric.Abortion,
					MedicinesAllergies = pb.MedicineAllergy,
					MedicinesAllergiesList = string.Join(", ", pb.AllergicMedicines!),
					Toxics = pb.Toxics,
					ToxicsList = string.Join(", ", pb.ToxicsList!),
					PassMedicines = string.Join(", ", pb.Medicines!),
					FamilyBackground = pb.FamilyBackground,
					FamilyBackgroundList = string.Join(", ", pb.FamilyBackgroundList!),
					ExamId = newExamToAdd.Id
				};
				await _dBContext.PersonalBackground.AddAsync(newPersonalBackgroundToAdd);
				await _dBContext.SaveChangesAsync();

				return BaseResponse<string>.GetSuccess("Ok", "created", HttpStatusCode.Created);
			}
			catch (Exception ex)
			{

                return BaseResponse<string>.GetError(ex.Message, HttpStatusCode.InternalServerError);
            }
		}

        public async Task<BaseResponse<SearchWithFilters<ExamDto>>> GetExamsWithFilters(int sliceIndex, int sliceSize, ExamSearchFilters searchFilters)
        {
			try
			{
				IQueryable<Exam> examsQuery = _dBContext.Exam.Include(e => e.personalBackground);

				if(searchFilters.DoctorId != 0)
				{
					examsQuery = examsQuery.Where(e => e.DoctorId == searchFilters.DoctorId);
				}
				if(searchFilters.PatientId != 0)
				{
					examsQuery = examsQuery.Where(e => e.PatientId == searchFilters.PatientId);
				}
				if(searchFilters.ExamId != 0)
				{
					examsQuery = examsQuery.Where(e => e.Id == searchFilters.ExamId);
				}

				int searchResults = await examsQuery.CountAsync();

				examsQuery = examsQuery.Skip((sliceIndex - 1) * sliceSize)
					.Take(sliceSize);

				List<ExamDto> examDtos = await examsQuery.Select(e => new ExamDto()
				{
					Id = e.Id,
					EvaluationDate = e.EvaluationDate,
					InterventionProposed = e.InterventionProposed,
					PersonalBackground = new PersonalBackgroundDto() {
						ArterialHypertension = e.personalBackground!.ArterialHypertension,
						MyocardialInfarction = e.personalBackground!.MyocardialInfarction,
						Asthma = e.personalBackground!.Asthma,
						Pneumopathy = e.personalBackground!.Pneumopathy,
						Dyslipidemia = e.personalBackground!.Dyslipidemia,
						Angina = e.personalBackground!.Angina,
						LiverDisease = e.personalBackground!.LiverDisease,
						Homeopathy = e.personalBackground!.Homeopathy,
						Stroke = e.personalBackground!.Stroke,
						DiabetesMellitus = e.personalBackground!.DiabetesMellitus,
						Chagas = e.personalBackground!.Chagas,
						Thyroidopathy = e.personalBackground!.Thyroidopathy,
						Surgery = e.personalBackground!.Surgery,
						SurgeryType = e.personalBackground!.SurgeryType,
						GestationWeeks = e.personalBackground!.GestationWeeks,
						For = e.personalBackground!.For,
						Caesarean = e.personalBackground!.Caesarean,
						Stillbirth = e.personalBackground!.Stillbirth,
						Abortion = e.personalBackground!.Abortion,
						MedicineAllergy = e.personalBackground!.MedicinesAllergies,
						AllergicMedicines = e.personalBackground!.MedicinesAllergiesList.Split(",", StringSplitOptions.TrimEntries).ToList(),
						Toxics = e.personalBackground!.Toxics,
						ToxicsList = e.personalBackground!.ToxicsList.Split(",", StringSplitOptions.TrimEntries).ToList(),
						OtherPersonalBackground = e.personalBackground!.OtherPersonalBackground.Split(",", StringSplitOptions.TrimEntries).ToList(),
						Medicines = e.personalBackground!.PassMedicines.Split(",", StringSplitOptions.TrimEntries).ToList(),
						FamilyBackground = e.personalBackground!.FamilyBackground,
						FamilyBackgroundList = e.personalBackground!.FamilyBackgroundList.Split(",", StringSplitOptions.TrimEntries).ToList()
					}
				}).ToListAsync();


				return BaseResponse<SearchWithFilters<ExamDto>>.GetSuccess("Ok",
					new SearchWithFilters<ExamDto>(examDtos, searchResults),
					HttpStatusCode.OK);
			}
			catch (Exception ex)
			{

				return BaseResponse<SearchWithFilters<ExamDto>>.GetError(ex.Message, HttpStatusCode.InternalServerError);
			}
        }
    }
}
