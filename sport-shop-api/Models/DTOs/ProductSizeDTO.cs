using System.ComponentModel.DataAnnotations;

namespace sport_shop_api.Models.DTOs
{
    public class ProductSizeDTO
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
    }
}
