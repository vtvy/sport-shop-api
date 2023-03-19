using System.ComponentModel.DataAnnotations;

namespace sport_shop_api.Models.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public int Quality { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
