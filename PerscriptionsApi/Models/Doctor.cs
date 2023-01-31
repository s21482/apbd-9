using System.ComponentModel.DataAnnotations;

namespace PerscriptionsApi.Models
{
    public class Doctor
    {
        [Key]
        public int IdDoctor { get; set; }
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public virtual ICollection<Prescription>? Prescriptions { get; set; }
    }
}
