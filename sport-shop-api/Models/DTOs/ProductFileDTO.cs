using System.ComponentModel.DataAnnotations;

namespace sport_shop_api.Models.DTOs
{
    public class ProductFileDTO
    {
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Price { get; set; }
        public IFormFile File { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
