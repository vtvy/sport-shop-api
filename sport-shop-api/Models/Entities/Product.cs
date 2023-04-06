using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sport_shop_api.Models.Entities
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        public string Url { get; set; } = string.Empty;
        [Required, Range(1, double.MaxValue)]
        public double Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public List<ProductSize> Sizes { get; set; }
    }
}
