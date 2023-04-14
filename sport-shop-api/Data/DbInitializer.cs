using sport_shop_api.Models.Entities;
using BC = BCrypt.Net.BCrypt;

namespace sport_shop_api.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IConfiguration configuration, AppDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any users.
            if (!context.Users.Any())
            {
                List<User> users = new()
                {
                    new User() { Email = configuration["Accounts:admin-email"], Password = BC.HashPassword(configuration["Accounts:admin-pass"]), Role = "Admin", Name = "Admin", Address = "CIT" },
                    new User() { Email = configuration["Accounts:vy-email"], Password = BC.HashPassword(configuration["Accounts:vy-pass"]), Role = "Admin", Name = "Trieu Vy", Address = "OFFICE"},
                };
                context.Users.AddRange(users);
                context.SaveChanges();
            }


            // Look for any category.
            if (!context.Categories.Any())
            {
                List<Category> categories = new()
            {
                new Category() { Name = "Sock" },
                new Category() { Name = "Shirt" },
            };
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            // Look for any product.
            if (!context.Products.Any())
            {
                List<Product> products = new()
            {
                new Product() { Name = "Cose", Quantity = 2000, Url = "https://www.rocconsulting.com/wp-content/uploads/2017/01/JDTrafford_1772.jpg", Price = 100000, Description = "This is very beautiful sock", CategoryId = 1 },
                new Product() { Name = "Lenvo", Quantity = 2000, Url = "https://www.rocconsulting.com/wp-content/uploads/2017/01/JDTrafford_1772.jpg", Price = 200000, Description = "This is very much pretty for boy", CategoryId = 2},
            };
                context.Products.AddRange(products);
                context.SaveChanges();
            }

            // Look for any product size.
            if (!context.ProductSizes.Any())
            {
                List<ProductSize> productSizes = new()
            {
                new ProductSize() { Name = "S", Price = 100000, ProductId = 1 },
                new ProductSize() { Name = "L", Price = 100000, ProductId = 1 },
                new ProductSize() { Name = "M", Price = 100000, ProductId = 1 },
                new ProductSize() { Name = "S", Price = 200000, ProductId = 2 },
                new ProductSize() { Name = "L", Price = 200000, ProductId = 2 },
                new ProductSize() { Name = "M", Price = 200000, ProductId = 2 },
            };
                context.ProductSizes.AddRange(productSizes);
                context.SaveChanges();
            }

        }
    }
}
