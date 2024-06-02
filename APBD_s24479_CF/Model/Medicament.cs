using System.ComponentModel.DataAnnotations;

namespace APBD_s24479_CF.Model;

public class Medicament
{
    [Key]
    public int IdMedicament { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(100)]
    public string Description { get; set; }

    [Required]
    [MaxLength(100)]
    public string Type { get; set; }

    public ICollection<PrescriptionMedicament> prescriptionMedicaments { get; set; }

}