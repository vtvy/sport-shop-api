using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sport_shop_api.Models.Entities
{
    [Table("HistoryProduct"), PrimaryKey(nameof(ProductSizeId), nameof(HistoryId))]
    public class HistoryProduct
    {
        public int ProductSizeId { get; set; }
        public int HistoryId { get; set; }
        [ForeignKey("ProductSizeId")]
        public ProductSize ProductSize { get; set; }
        [ForeignKey("HistoryId")]
        public History History { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
