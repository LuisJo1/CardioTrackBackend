using CardioTrackAPI.Data;
using CardioTrackAPI.Model;
using CardioTrackAPI.Services;
using System.Text;
using System.Text.Json;

namespace CardioTrackAPI
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();
			builder.Services.AddSwaggerGen();
			builder.Services.AddDbContext<DBContext>();
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddDataProtection();

			builder.Services.AddScoped<IDoctorService, DoctorService>();
			builder.Services.AddScoped<IPatientService, PatientService>();
			builder.Services.AddScoped<IExamService, ExamService>();
			builder.Services.AddScoped<IForgotPasswordService, ForgotPasswordService>();

			builder.Services.AddAuthentication("cookie")
				.AddCookie("cookie", options =>
				{
					options.Events.OnRedirectToAccessDenied = context =>
					{
						context.Response.StatusCode = 403;
						var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(BaseResponse<string>.GetError("User doesn't meet the required permissions", System.Net.HttpStatusCode.Forbidden)));
						context.Response.ContentType = "application/json";
						context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
						return Task.CompletedTask;
					};

					options.Events.OnRedirectToLogin = context =>
					{
						context.Response.StatusCode = 401;
						var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(BaseResponse<string>.GetError("Unauthorized", System.Net.HttpStatusCode.Unauthorized)));
						context.Response.ContentType = "application/json";
						context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
						return Task.CompletedTask;
					};
				});

			builder.Services.AddAuthorization(options =>
			{
				options.AddPolicy("default-policy", policy =>
				{
					policy.RequireAuthenticatedUser();
					policy.AuthenticationSchemes = new List<string>() { "cookie" };
				});
			});

			builder.Services.AddCors(pb =>
			{
				pb.AddPolicy("app-cors", options =>
				{
					options.WithOrigins("http://localhost:4200");
					options.AllowCredentials();
					options.AllowAnyHeader();
					options.AllowAnyMethod();
				});
			});

			var app = builder.Build();

			if(app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			using (var scope = app.Services.CreateScope())
			{

				var services = scope.ServiceProvider;
				DBContext dBContext = services.GetRequiredService<DBContext>();
				await SeedData.LoadDataAsync(dBContext);
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.UseAuthentication();

			app.UseCors("app-cors");

			app.UseAuthorization();

			app.Run();

		}
	}
}
