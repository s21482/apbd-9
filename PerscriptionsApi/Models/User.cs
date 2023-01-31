using System.ComponentModel.DataAnnotations;

namespace PerscriptionsApi.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }
        [Required]
        [MaxLength(30)]
        public string Login { get; set; }
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public string RefreshToken { get; set; }
        [Required]
        public DateTime? RefreshTokenExpirationDate { get; set; }
    }
}
