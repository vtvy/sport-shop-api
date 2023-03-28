using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace sport_shop_api.Models.Entities
{
    [Table("User"), Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; } = "Unknown";
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; } = "User";
        public string Address { get; set; }
        public List<History> Histories { get; set; }
    }
}
