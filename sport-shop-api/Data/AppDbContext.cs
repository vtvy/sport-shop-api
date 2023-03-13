using Microsoft.EntityFrameworkCore;
using sport_shop_api.Models.Entities;

namespace sport_shop_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { set; get; }
        public DbSet<RefreshToken> RefreshTokens { set; get; }
        public DbSet<Category> Categories { set; get; }
        public DbSet<History> Histories { set; get; }
        public DbSet<Product> Products { set; get; }
        public DbSet<ProductImage> ProductImages { set; get; }
        public DbSet<ProductSize> ProductSizes { set; get; }
        public DbSet<HistoryProduct> HistoryProducts { set; get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }

}