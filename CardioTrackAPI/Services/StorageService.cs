using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using CardioTrackAPI.Data;
using CardioTrackAPI.Model;
using CTalk.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CTalk.AppServices
{
    public class StorageService : IStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IStorageService> _logger;
        private readonly DBContext _dBContext;
        private IAmazonS3 _awsS3Client;

        public StorageService(IConfiguration configuration, ILogger<IStorageService> logger, DBContext dBContext)
        {
            AWSConfigsS3.UseSignatureVersion4 = true;
            _awsS3Client = new AmazonS3Client(
                Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"), Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"),
                new AmazonS3Config()
                {
                    ServiceURL = string.Format("https://s3.filebase.com"),
                    ForcePathStyle = true
                });
            _configuration = configuration;
            _logger = logger;
            _dBContext = dBContext;
        }

        public async Task<BaseResponse<List<string>>> GetBlogImagesUrlAysnc(Guid postId)
        {
            try
            {
                ListObjectsRequest listObjectsRequest = new()
                {
                    BucketName = _configuration.GetValue<string>("AWS:BucketName"),
                    Prefix = $"patientPhoto/{postId}"
                };
                ListObjectsResponse listObjectsResponse = await _awsS3Client.ListObjectsAsync(listObjectsRequest);
                return BaseResponse<List<string>>.GetSuccess("Ok", new List<string>() { "Epa " });
            }
            catch (Exception ex)
            {
                return BaseResponse<List<string>>.GetError(ex.Message);
            }
        }

        public async Task<BaseResponse<string>> GetObjectPresignedUrl(string objectKey)
        {
            try
            {
                GetPreSignedUrlRequest getPreSignedUrlRequest = new()
                {
                    BucketName = _configuration.GetValue<string>("AWS:BucketName")!,
                    Key = objectKey,
                    Expires = DateTime.Now.AddDays(7)
                };
                string presignedUrl = await _awsS3Client.GetPreSignedURLAsync(getPreSignedUrlRequest);
                return BaseResponse<string>.GetSuccess("Ok", presignedUrl);
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.GetError(ex.Message);
            }
        }

        public async Task<BaseResponse<string>> UploadImagesAsync(List<IFormFile> formImages, long patientId)
        {
            try
            {
                Patient? patient = await _dBContext.Patient.FindAsync(patientId);
                if (patient == null)
                {
                    return BaseResponse<string>.GetError("Coulnd't find a user with the given ID", HttpStatusCode.NotFound);
                }
                if (!string.IsNullOrEmpty(patient.ProfileImgS3Key))
                {
                    DeleteObjectRequest deleteObjectRequest = new()
                    {
                        BucketName = _configuration.GetValue<string>("AWS:BucketName")!,
                        Key = patient.ProfileImgS3Key,
                    };

                    var deleteResp = await _awsS3Client.DeleteObjectAsync(deleteObjectRequest);
                    if ((int)deleteResp.HttpStatusCode > 300)
                    {
                        return BaseResponse<string>.GetError("Coulnd't delete previous image", HttpStatusCode.InternalServerError);
                    }
                }
                foreach (IFormFile formImage in formImages)
                {
                    PutObjectRequest putObjectRequest = new()
                    {
                        InputStream = formImage.OpenReadStream(),
                        BucketName = _configuration.GetValue<string>("AWS:BucketName")!,
                        Key = $"patientPhoto/{patientId}/{formImage.FileName}",
                        ContentType = formImage.ContentType
                    };

                    PutObjectResponse uploadResponse = await _awsS3Client.PutObjectAsync(putObjectRequest);
                }
                ListObjectsRequest listObjectsRequest = new()
                {
                    BucketName = _configuration.GetValue<string>("AWS:BucketName"),
                    Prefix = $"patientPhoto/{patientId}"
                };
                ListObjectsResponse listObjectsResponse = await _awsS3Client.ListObjectsAsync(listObjectsRequest);
                List<PostMediaItem>? postMedia = await GenerateMediaPresignedUrls(listObjectsResponse.S3Objects);
                StorePresignedUrlsRequest StorePresignedUrlsRequest = new()
                {
                    PatientId = patientId,
                    LastPresignedUrlRequestUTC = DateTimeOffset.UtcNow,
                    PostMedia = postMedia
                };
                await StoreMediaPresignedUrlsAsync(StorePresignedUrlsRequest);
                return BaseResponse<string>.GetSuccess("patched", postMedia![0].Url, HttpStatusCode.OK);
            }
            catch (AmazonS3Exception amazonS3Ex)
            {
                return BaseResponse<string>.GetError(amazonS3Ex.Message, HttpStatusCode.InternalServerError);
            }
        }
        private async Task<List<PostMediaItem>?> GenerateMediaPresignedUrls(List<S3Object> postMediaObjects)
        {
            try
            {
                List<PostMediaItem> presignedUrls = [];
                foreach (S3Object postMediaObject in postMediaObjects)
                {
                    GetPreSignedUrlRequest getPreSignedUrlRequest = new()
                    {
                        BucketName = _configuration.GetValue<string>("AWS:BucketName")!,
                        Key = postMediaObject.Key,
                        Expires = DateTime.Now.AddDays(7)
                    };
                    string presignedUrl = await _awsS3Client.GetPreSignedURLAsync(getPreSignedUrlRequest);
                    GetObjectMetadataResponse index = await _awsS3Client.GetObjectMetadataAsync(new GetObjectMetadataRequest
                    {
                        BucketName = _configuration.GetValue<string>("AWS:BucketName")!,
                        Key = postMediaObject.Key
                    });
                    presignedUrls.Add(new PostMediaItem { Url = presignedUrl, Key = postMediaObject.Key });
                }
                return presignedUrls;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
        public async Task<BaseResponse<bool>> StoreMediaPresignedUrlsAsync(StorePresignedUrlsRequest storePresignedUrlsRequest)
        {
            try
            {
                if (storePresignedUrlsRequest.PostMedia is null)
                    return BaseResponse<bool>.GetError("Post media urls are null");
                foreach (PostMediaItem media in storePresignedUrlsRequest.PostMedia)
                {
                    var p = await _dBContext.Patient.Where(p => p.Id == storePresignedUrlsRequest.PatientId)
                        .FirstOrDefaultAsync();
                    if (p is null)
                    {
                        return BaseResponse<bool>.GetError("Couldn't find a user with the given ID", HttpStatusCode.NotFound);
                    }
                    p.ProfileImgUrl = media.Url;
                    p.ProfileImgS3Key = media.Key;
                    p.ProfileImgUrlLastGenerationTime = DateTime.Now;
                }
                await _dBContext.SaveChangesAsync();
                return BaseResponse<bool>.GetSuccess("Ok", true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.GetError(ex.Message);
            }
        }

        public async Task<BaseResponse<string>> UpdateMediaPresignedUrlAsync(long patientId, string objectKey)
        {
            try
            {
                GetPreSignedUrlRequest getPreSignedUrlRequest = new()
                {
                    BucketName = _configuration.GetValue<string>("AWS:BucketName")!,
                    Key = objectKey,
                    Expires = DateTime.Now.AddDays(7)
                };
                string presignedUrl = await _awsS3Client.GetPreSignedURLAsync(getPreSignedUrlRequest);
                var p = await _dBContext.Patient.Where(p => p.Id == patientId)
                    .FirstOrDefaultAsync();
                if (p is null)
                {
                    return BaseResponse<string>.GetError("Couldn't find a user with the given ID", HttpStatusCode.NotFound);
                }
                p.ProfileImgUrl = presignedUrl;
                p.ProfileImgUrlLastGenerationTime = DateTime.Now;
                return BaseResponse<string>.GetSuccess("Patched", "Ok", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.GetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task<BaseResponse<string>> DeletePatientProfilePic(long patientId)
        {
            try
            {
                Patient? patient = await _dBContext.Patient.FindAsync(patientId);
                if (patient == null)
                {
                    return BaseResponse<string>.GetError("Coulnd't find a user with the given ID", HttpStatusCode.NotFound);
                }
                if (!string.IsNullOrEmpty(patient.ProfileImgS3Key))
                {
                    DeleteObjectRequest deleteObjectRequest = new()
                    {
                        BucketName = _configuration.GetValue<string>("AWS:BucketName")!,
                        Key = patient.ProfileImgS3Key,
                    };

                    var deleteResp = await _awsS3Client.DeleteObjectAsync(deleteObjectRequest);
                    if ((int)deleteResp.HttpStatusCode > 300)
                    {
                        return BaseResponse<string>.GetError("Coulnd't delete previous image", HttpStatusCode.InternalServerError);
                    }
                    patient.ProfileImgUrlLastGenerationTime = DateTime.MinValue;
                    patient.ProfileImgUrl = "";
                    patient.ProfileImgS3Key = "";

                    await _dBContext.SaveChangesAsync();
                }
                return BaseResponse<string>.GetSuccess("deleted", "Ok", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.GetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
