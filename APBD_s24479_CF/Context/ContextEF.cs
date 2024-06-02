using APBD_s24479_CF.Model;
using Microsoft.EntityFrameworkCore;

namespace APBD_s24479_CF.Context
{
    public partial class ContextEF : DbContext
    {
        public ContextEF() 
        { 
        }

        public ContextEF(DbContextOptions<ContextEF> options) : base(options) 
        { 
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=localhost,1433;Initial Catalog=master;User=SA;Password=Password12345;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureModels(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureModels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Prescription>().HasKey(p => p.IdPrescription);
            modelBuilder.Entity<Medicament>().HasKey(m => m.IdMedicament);
            modelBuilder.Entity<PrescriptionMedicament>().HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });

            modelBuilder.Entity<PrescriptionMedicament>()
                .HasOne(pm => pm.Medicament)
                .WithMany(m => m.prescriptionMedicaments)
                .HasForeignKey(pm => pm.IdMedicament);

            modelBuilder.Entity<PrescriptionMedicament>()
                .HasOne(pm => pm.Prescription)
                .WithMany(p => p.prescriptionMedicaments)
                .HasForeignKey(pm => pm.IdPrescription);

            modelBuilder.Entity<Patient>()
                .Property(p => p.IdPatient)
                .ValueGeneratedNever();
            
            modelBuilder.Entity<Doctor>()
                .Property(d => d.IdDoctor)
                .ValueGeneratedNever();
        }
    }
}
