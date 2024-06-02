using APBD_s24479_CF.Context;
using APBD_s24479_CF.DTOModel;
using APBD_s24479_CF.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace APBD_s24479_CF.Service
{
    public interface IPrescriptionsService
    {
        IActionResult AddPrescription(PrescriptionDTO prescription);
    }

    public class PrescriptionsService : IPrescriptionsService
    {
        private readonly ContextEF _contextEf;

        public PrescriptionsService(ContextEF contextEf)
        {
            _contextEf = contextEf;
        }

        public IActionResult AddPrescription(PrescriptionDTO newPrescription)
        {
            if (IsInvalidPrescriptionDate(newPrescription))
            {
                return new BadRequestObjectResult("Bad date");
            }

            if (HasTooManyMedicaments(newPrescription))
            {
                return new BadRequestObjectResult("ERROR: Too many meds");
            }

            if (ContainsInvalidMedicaments(newPrescription, out var missingMedicamentIds))
            {
                return new BadRequestObjectResult($"Medicament with this ID doesn't exist: {string.Join(", ", missingMedicamentIds)}");
            }

            EnsurePatientExists(newPrescription.Patient);
            EnsureDoctorExists(newPrescription.Doctor);

            var prescriptionEntity = AddNewPrescription(newPrescription);
            AddPrescriptionMedicaments(newPrescription, prescriptionEntity.IdPrescription);

            return new OkObjectResult("Added");
        }

        private bool IsInvalidPrescriptionDate(PrescriptionDTO prescription)
        {
            return prescription.DueDate <= prescription.Date;
        }

        private bool HasTooManyMedicaments(PrescriptionDTO prescription)
        {
            return prescription.Medicament != null && prescription.Medicament.Count() >= 10;
        }

        private bool ContainsInvalidMedicaments(PrescriptionDTO prescription, out List<int> missingMedicamentIds)
        {
            var medicamentIds = prescription.Medicament?.Select(m => m.IdMedicament).ToList() ?? new List<int>();
            var existingMedicamentIds = _contextEf.Medicaments.Select(m => m.IdMedicament).ToList();
            missingMedicamentIds = medicamentIds.Except(existingMedicamentIds).ToList();
            return missingMedicamentIds.Any();
        }

        private void EnsurePatientExists(PatientDTO patientDto)
        {
            var patient = _contextEf.Patients.FirstOrDefault(p => p.IdPatient == patientDto.IdPatient);
            if (patient == null)
            {
                _contextEf.Patients.Add(new Patient
                {
                    IdPatient = patientDto.IdPatient,
                    FirstName = patientDto.FirstName,
                    LastName = patientDto.LastName,
                    Birthdate = patientDto.Birthdate,
                });
                _contextEf.SaveChanges();
            }
        }

        private void EnsureDoctorExists(DoctorDTO doctorDto)
        {
            var doctor = _contextEf.Doctors.FirstOrDefault(d => d.IdDoctor == doctorDto.IdDoctor);
            if (doctor == null)
            {
                _contextEf.Doctors.Add(new Doctor
                {
                    IdDoctor = doctorDto.IdDoctor,
                    FirstName = doctorDto.FirstName,
                    LastName = doctorDto.LastName,
                    Email = doctorDto.Email,
                });
                _contextEf.SaveChanges();
            }
        }

        private Prescription AddNewPrescription(PrescriptionDTO prescriptionDto)
        {
            var newPrescription = new Prescription
            {
                Date = prescriptionDto.Date,
                DueDate = prescriptionDto.DueDate,
                IdPatient = prescriptionDto.Patient.IdPatient,
                IdDoctor = prescriptionDto.Doctor.IdDoctor,
            };

            var addedEntity = _contextEf.Prescriptions.Add(newPrescription);
            _contextEf.SaveChanges();
            return addedEntity.Entity;
        }

        private void AddPrescriptionMedicaments(PrescriptionDTO prescriptionDto, int prescriptionId)
        {
            var prescriptionMedicaments = prescriptionDto.Medicament.Select(m => new PrescriptionMedicament
            {
                IdMedicament = m.IdMedicament,
                IdPrescription = prescriptionId,
                Dose = m.Dose,
                Details = m.Details,
            }).ToList();

            _contextEf.PrescriptionMedicaments.AddRange(prescriptionMedicaments);
            _contextEf.SaveChanges();
        }
    }
}
