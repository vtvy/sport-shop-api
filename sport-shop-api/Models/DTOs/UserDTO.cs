using System.ComponentModel.DataAnnotations;
namespace sport_shop_api.Models.DTOs
{
    public class UserDTO
    {
        [Required]
        public string Email { get; set; }
        [Required, MaxLength(100)]
        public string Password { get; set; }
        public string Name { get; set; } = "Unknown";
        public string Address { get; set; }
    }
}
