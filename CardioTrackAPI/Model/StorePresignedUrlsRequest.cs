namespace CTalk.Models
{
	public class StorePresignedUrlsRequest
	{
		public long PatientId { get; set; }
		public List<PostMediaItem>? PostMedia { get; set; }
		public DateTimeOffset LastPresignedUrlRequestUTC { get; set; }
	}
	public class PostMediaItem
	{
		public string Key { get; set; } = string.Empty;
		public string Url { get; set; } = string.Empty;
	}
}
