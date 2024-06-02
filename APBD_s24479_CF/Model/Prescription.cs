using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_s24479_CF.Model;

public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [Column(TypeName = "Date")]
    public DateTime Date { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [Column(TypeName = "Date")]
    public DateTime DueDate { get; set; }

    public int IdPatient { get; set; }
    [ForeignKey(nameof(IdPatient))]
    public Patient Patients { get; set; }

    public int IdDoctor { get; set; }
    [ForeignKey(nameof(IdDoctor))]
    public Doctor Doctors { get; set; }

    public ICollection<PrescriptionMedicament> prescriptionMedicaments { get; set; }

}