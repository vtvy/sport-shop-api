using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sport_shop_api.Models.Entities
{
    [Table("RefreshToken"),]
    public class RefreshToken
    {
        [Key]
        public int RefreshTokenId { get; set; }
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public bool IsUsed { get; set; }
        [Required]
        public DateTime ExpiredAt { get; set; }
    }
}
