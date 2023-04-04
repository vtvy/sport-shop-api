using System.ComponentModel.DataAnnotations;

namespace sport_shop_api.Models.DTOs
{
    public class ProductFileDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        [Required, Range(0, double.MaxValue)]
        public double Price { get; set; }
        public IFormFile File { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
