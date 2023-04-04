using System.ComponentModel.DataAnnotations;

namespace sport_shop_api.Models.DTOs
{
    public class ProductSizeDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, Range(0, double.MaxValue)]
        public double Price { get; set; }
    }
}
