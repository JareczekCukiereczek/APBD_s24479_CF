using Microsoft.AspNetCore.Mvc;
using APBD_s24479_CF.Context;
using APBD_s24479_CF.DTOModel;
using System.Linq;
using APBD_s24479_CF.Model;

namespace APBD_s24479_CF.Service
{
    public interface IPatientsService
    {
        IActionResult GetPatientData(int patientId);
    }

    public class PatientsService : IPatientsService
    {
        private readonly ContextEF _contextEf;

        public PatientsService(ContextEF contextEf)
        {
            _contextEf = contextEf;
        }

        public IActionResult GetPatientData(int patientId)
        {
            if (IsInvalidPatientId(patientId))
            {
                return new BadRequestObjectResult("Invalid patient ID");
            }

            var patientData = FetchPatientData(patientId);

            if (patientData == null)
            {
                return new NotFoundObjectResult("Patient not found");
            }

            return new OkObjectResult(patientData);
        }

        private bool IsInvalidPatientId(int patientId)
        {
            return patientId <= 0;
        }

        private PrescriptionDTO FetchPatientData(int patientId)
        {
            return _contextEf.Prescriptions
                .Where(p => p.IdPatient == patientId)
                .Select(p => new PrescriptionDTO
                {
                    Patient = CreatePatientDTO(p),
                    Doctor = CreateDoctorDTO(p),
                    Medicament = CreateMedicamentDTOList(p)
                })
                .FirstOrDefault();
        }

        private PatientDTO CreatePatientDTO(Prescription p)
        {
            return new PatientDTO
            {
                IdPatient = p.Patients.IdPatient,
                FirstName = p.Patients.FirstName,
                LastName = p.Patients.LastName,
                Birthdate = p.Patients.Birthdate,
            };
        }

        private DoctorDTO CreateDoctorDTO(Prescription p)
        {
            return new DoctorDTO
            {
                IdDoctor = p.Doctors.IdDoctor,
                FirstName = p.Doctors.FirstName,
                LastName = p.Doctors.LastName,
                Email = p.Doctors.Email,
            };
        }

        private List<MedicamentDTO> CreateMedicamentDTOList(Prescription p)
        {
            return p.prescriptionMedicaments
                .Select(pm => new MedicamentDTO
                {
                    IdMedicament = pm.Medicament.IdMedicament,
                    Name = pm.Medicament.Name,
                    Dose = pm.Dose ?? 0,
                    Description = pm.Medicament.Description,
                })
                .ToList();
        }
    }
}
