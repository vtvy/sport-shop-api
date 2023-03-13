using System.ComponentModel.DataAnnotations;

namespace sport_shop_api.Models.DTOs
{
    public class TokenDTO
    {
        [Required]
        public string access_token { get; set; }
        [Required]
        public string refresh_token { get; set; }
    }
}
