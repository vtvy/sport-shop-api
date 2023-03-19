using System.ComponentModel.DataAnnotations;

namespace sport_shop_api.Models.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
