using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PerscriptionsApi.Models
{
    [PrimaryKey(nameof(IdMedicament), nameof(IdPrescription))]
    public class Prescription_Medicament
    {
        public int Dose { get; set; }
        [Required]
        [MaxLength(100)]
        public string Details { get; set; }
        public int IdMedicament { get; set; }
        public int IdPrescription { get; set; }
        [ForeignKey(nameof(IdMedicament))]
        public virtual Medicament Medicament { get; set; }
        [ForeignKey(nameof(IdPrescription))]
        public virtual Prescription Prescription { get; set; }
    }
}
