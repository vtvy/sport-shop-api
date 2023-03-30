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
                new User() { Email = configuration["Accounts:admin-email"], Password = BC.HashPassword(configuration["Accounts:admin-pass"]), Role = "Admin", Name = "Admin" },
                new User() { Email = configuration["Accounts:vy-email"], Password = BC.HashPassword(configuration["Accounts:vy-pass"]), Role = "Admin", Name = "Trieu Vy"},
            };

                context.Users.AddRange(users);
                context.SaveChanges();
            }

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

            if (!context.Products.Any())
            {
                List<Product> products = new()
            {
                new Product() { Name = "Conse", Quantity = 1000, Price = 100000, Description = "Very beautiful",
                    CategoryId = 1, Url = "https://www.rocconsulting.com/wp-content/uploads/2017/01/JDTrafford_1772.jpg"
                 },
                new Product() { Name = "Shirt", Quantity = 2000, Price = 200000, Description = "Very good",
                    CategoryId = 2, Url = "https://www.rocconsulting.com/wp-content/uploads/2017/01/JDTrafford_1772.jpg" },
            };
                context.Products.AddRange(products);
                context.SaveChanges();
            }

            return;   // DB has been seeded
        }
    }
}
