using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sport_shop_api.Models.Entities
{
    [Table("History")]
    public class History
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public bool OnDelivery { get; set; } = false;
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public List<HistoryProduct> HistoryProducts { get; set; }
    }
}
