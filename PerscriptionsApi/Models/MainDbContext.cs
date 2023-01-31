using Microsoft.EntityFrameworkCore;

namespace PerscriptionsApi.Models
{
    public class MainDbContext : DbContext
    {
        public MainDbContext()
        { }

        public MainDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Prescription_Medicament> Prescription_Medicaments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                        .UseLazyLoadingProxies()
                        //.LogTo(Console.WriteLine)
                        .UseSqlServer("Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s21482;Integrated Security=True;Encrypt=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(opt =>
            {
                opt.HasData(
                        new Patient { IdPatient = 1, FirstName = "Mateusz", LastName = "Dziuba", BirthDate = new System.DateTime(1991, 7, 14) }
                       );
            });

            modelBuilder.Entity<Doctor>(opt =>
            {
                opt.HasData(
                        new Doctor
                        {
                            IdDoctor = 1,
                            FirstName = "Jan",
                            LastName = "Kowalski",
                            Email = "jan.kowalski@szpital.pl"
                        },
                        new Doctor
                        {
                            IdDoctor = 2,
                            FirstName = "Zbigniew",
                            LastName = "Religa",
                            Email = "zbigniew.religa@serce.pl"
                        });
            });

            modelBuilder.Entity<Medicament>(opt =>
            {
                opt.HasData(
                        new Medicament
                        {
                            IdMedicament = 1,
                            Name = "Amoxyklaw",
                            Description = "Zakażenie dróg oddechowych",
                            Type = "Antybiotyk"
                        },
                        new Medicament
                        {
                            IdMedicament = 2,
                            Name = "Nimesil",
                            Description = "Bóle stawów",
                            Type = "Lek przeciwbólowy"
                        });
            });

            modelBuilder.Entity<Prescription>(opt =>
            {
                opt.HasData(
                        new Prescription
                        {
                            IdPrescription = 1,
                            Date = new System.DateTime(2023, 1, 30),
                            DueDate = new System.DateTime(2023, 3, 1),
                            IdPatient = 1,
                            IdDoctor = 1
                        });

            });

            modelBuilder.Entity<Prescription_Medicament>(opt =>
            {
                opt.HasData(
                        new Prescription_Medicament
                        {
                            IdMedicament = 1,
                            IdPrescription = 1,
                            Dose = 50,
                            Details = "Raz dziennie",
                        },
                        new Prescription_Medicament
                        {
                            IdMedicament = 2,
                            IdPrescription = 1,
                            Dose = 20,
                            Details = "Po każdym posiłku",
                        }
                );

            });
        }
    }
}
