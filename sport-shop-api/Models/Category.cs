﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sport_shop_api.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public List<Product> Products { get; set; }
    }
}
