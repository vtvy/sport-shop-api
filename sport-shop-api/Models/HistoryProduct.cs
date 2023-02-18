using System.ComponentModel.DataAnnotations.Schema;

namespace sport_shop_api.Models
{
    [Table("HistoryProduct")]
    public class HistoryProduct
    {
        public int ProductSizeId { get; set; }
        public int HistoryId { get; set; }

        [ForeignKey("ProductSizeId")]
        public ProductSize ProductSize { get; set; }

        [ForeignKey("HistoryId")]
        public History History { get; set; }
        public int Quantity { get; set; }
    }
}
