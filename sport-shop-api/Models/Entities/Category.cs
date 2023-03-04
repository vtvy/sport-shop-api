using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sport_shop_api.Models.Entities
{
    [Table("Category"), Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }
}
