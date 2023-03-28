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
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            List<User> users = new List<User>()
            {
                new User() { Email = configuration["Accounts:admin-email"], Password = BC.HashPassword(configuration["Accounts:admin-pass"]), Role = "Admin", Name = "Admin" },
                new User() { Email = configuration["Accounts:vy-email"], Password = BC.HashPassword(configuration["Accounts:vy-pass"]), Role = "Admin", Name = "Trieu Vy"},
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
