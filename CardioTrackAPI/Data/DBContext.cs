using CardioTrackAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CardioTrackAPI.Data
{
	public class DBContext : DbContext
	{
		private readonly IConfiguration _configuration;

		public DBContext(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
		}
		public DbSet<User> User { get; set; }
		public DbSet<Rol> Rol { get; set; }
		public DbSet<Doctor> Doctor { get; set; }
		public DbSet<Patient> Patient { get; set; }
		public DbSet<Exam> Exam { get; set; }
		public DbSet<DoctorPatients> DoctorPatients { get; set; }
		public DbSet<Treatment> Treatment { get; set; }
		public DbSet<TreatmentMedicines> TreatmentMedicine { get; set; }
		public DbSet<PersonalBackground> PersonalBackground { get; set; }
		public DbSet<PasswordRecoverToken> PasswordRecoverToken { get; set; }

	}
}
