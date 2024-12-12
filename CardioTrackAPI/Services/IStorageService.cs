using Amazon.S3;
using CardioTrackAPI.Model;
using CTalk.Models;

namespace CTalk.AppServices
{
	public interface IStorageService
	{
		Task<BaseResponse<string>> UploadImagesAsync(List<IFormFile> formImages, long patientId);
		Task<BaseResponse<string>> GetObjectPresignedUrl(string objectKey);
		Task<BaseResponse<bool>> StoreMediaPresignedUrlsAsync(StorePresignedUrlsRequest storePresignedUrlsRequest);
		Task<BaseResponse<string>> UpdateMediaPresignedUrlAsync(long patientId, string objectKey);
		Task<BaseResponse<string>> DeletePatientProfilePic(long patientId);
	}
}
