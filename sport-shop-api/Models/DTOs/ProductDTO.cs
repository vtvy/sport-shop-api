using System.ComponentModel.DataAnnotations;

namespace sport_shop_api.Models.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public int Quality { get; set; }
        [Required]
        public IFormFile files { get; set; }
        [Required]
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
