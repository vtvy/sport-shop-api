using Microsoft.EntityFrameworkCore;

namespace sport_shop_api.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { set; get; }
        public DbSet<Category> Categories { set; get; }
        public DbSet<History> Histories { set; get; }
        public DbSet<Product> Products { set; get; }
        public DbSet<ProductImage> ProductImages { set; get; }
        public DbSet<ProductSize> ProductSizes { set; get; }
        public DbSet<HistoryProduct> HistoryProducts { set; get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            builder.Entity<HistoryProduct>().HasKey(p => new { p.HistoryId, p.ProductSizeId });

            builder.Entity<User>(entity =>
            {
                entity.HasIndex(p => p.Email)
                      .IsUnique();
            });
        }

    }

}