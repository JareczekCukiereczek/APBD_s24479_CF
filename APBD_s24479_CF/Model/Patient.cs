using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_s24479_CF.Model;

public class Patient
{
    [Key] 
     public int IdPatient { get; set; }
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [Column(TypeName = "Date")]
    public DateTime Birthdate { get; set; }

    public ICollection<Prescription> Prescriptions { get; set; }

}