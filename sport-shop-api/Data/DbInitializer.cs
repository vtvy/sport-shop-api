using sport_shop_api.Models;

namespace sport_shop_api.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any users.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            List<User> users = new List<User>()
            {
                new User() { Email = "admin@gmail.com", Password = "strong", Role = "Admin" },
                new User() { Email = "vy@gmail.com", Password = "1234", Role = "User"},
                new User() { Email = "phuong@gmail.com", Password = "1234", Role = "User"},
                new User() { Email = "minh@gmail.com", Password = "1234", Role = "User"},
                new User() { Email = "khoi@gmail.com", Password = "1234", Role = "User"},
                new User() { Email = "ronaldo@gmail.com", Password = "1234", Role = "User"},
                new User() { Email = "messi@gmail.com", Password = "1234", Role = "User"},
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
