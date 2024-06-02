using System.ComponentModel.DataAnnotations;

namespace APBD_s24479_CF.Model;

public class PrescriptionMedicament
{
    public int IdMedicament { get; set; }
    public Medicament Medicament { get; set; }

    public int IdPrescription { get; set; }
    public Prescription Prescription { get; set; }

        
    public int? Dose { get; set; }
    [Required]
    [MaxLength(100)]
    public string Details { get; set; }
}