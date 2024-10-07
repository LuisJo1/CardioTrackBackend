using System.Net;

namespace CardioTrackAPI.Model
{
	public class BaseResponse<TData>
	{
		public TData? Data { get; set; }
		public string Message { get; set; } = string.Empty;
		public bool Success { get; set; }
		public HttpStatusCode? StatusCode { get; set; }
		public static BaseResponse<TData> GetSuccess(string message, TData data)
		{
			return new BaseResponse<TData>
			{
				Data = data,
				Message = message,
				Success = true
			};
		}
		public static BaseResponse<TData> GetSuccess(string message, TData data, HttpStatusCode statusCode)
		{
			return new BaseResponse<TData>
			{
				Data = data,
				Message = message,
				Success = true,
				StatusCode = statusCode
			};
		}
		public static BaseResponse<TData> GetError(string message)
		{
			return new BaseResponse<TData>
			{
				Message = message,
				Success = false
			};
		}
		public static BaseResponse<TData> GetError(string message, HttpStatusCode statusCode)
		{
			return new BaseResponse<TData>
			{
				Message = message,
				Success = false,
				StatusCode = statusCode
			};
		}
	}
}
