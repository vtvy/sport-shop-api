using System.ComponentModel.DataAnnotations;

namespace sport_shop_api.Models.DTOs
{
    public class UserLoginDTO
    {

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
