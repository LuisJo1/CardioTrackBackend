using CardioTrackAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CardioTrackAPI.Data
{
	public class SeedData
	{
		public static async Task LoadDataAsync(DBContext dBContext)
		{
			Rol? adminRol = await dBContext.Rol.FindAsync(1);
			if(adminRol == null)
			{
				dBContext.Rol.Add(new Rol { Name = "Admin" });
			}
			Rol? doctorRol = await dBContext.Rol.FindAsync(2);
			if (doctorRol == null)
			{
				dBContext.Rol.Add(new Rol { Name = "Doctor" });
			}
			Rol? patientRol = await dBContext.Rol.FindAsync(3);
			if (patientRol == null)
			{
				dBContext.Rol.Add(new Rol { Name = "Patient" });
			}
				await dBContext.SaveChangesAsync();



			User? adminUser = await dBContext.User
				.FirstOrDefaultAsync(u => u.Email == "cardiotrackofficial@gmail.com");
			if (adminUser == null)
			{
				dBContext.User.Add(new User
				{
					Email = "cardiotrackofficial@gmail.com",
					Password = BCrypt.Net.BCrypt.HashPassword("_admin012.."),
					RolId = 1
				});
				await dBContext.SaveChangesAsync();
			}
		}
	}
}
