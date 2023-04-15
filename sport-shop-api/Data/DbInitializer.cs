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
                    new User() { Email = "test@gmail.com", Password = BC.HashPassword(configuration["Accounts:vy-pass"]), Role = "User", Name = "Nguyen Van A", Address = "3/2  Hung Loi, Ninh Kieu, Can Tho"},
                };
                context.Users.AddRange(users);
                context.SaveChanges();
            }


            // Look for any category.
            if (!context.Categories.Any())
            {
                List<Category> categories = new()
            {
                new Category() { Name = "Giày nam" },
                new Category() { Name = "Quần áo nam" },
                new Category() { Name = "Phụ kiện nam" },
                new Category() { Name = "Dép nam" },
                new Category() { Name = "Giày nữ" },
                new Category() { Name = "Quần áo nữ" },
                new Category() { Name = "Phụ kiện nữ" },
                new Category() { Name = "Dép nữ" },
            };
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            // Look for any product.
            if (!context.Products.Any())
            {
                List<Product> products = new()
            {
                new Product() { Name = "Giày bóng đá nike nam", Quantity = 2000, Url = "https://product.hstatic.net/200000174405/product/1_75d9c28a608a4cd6b2101d430050092f_master.jpg", Price = 200000, Description = "Bộ đệm cao su cao cấp cho khả năng đàn hồi khi dậm nhảy. Phần gót giày cũng được làm bằng chất liệu vả đem lại sự thoải mái khi mang vào chân.", CategoryId = 1 },
                new Product() { Name = "Giày Tennis JET TERE ALL COURT MEN", Quantity = 2000, Url = "https://product.hstatic.net/200000174405/product/30s23649-jet_tere_all_court_men-3027-4-3_4_copy_fc8d48950acc4528aef1bbddc92349ef_1024x1024.jpg", Price = 150000, Description = "Khuyến mại vợt", CategoryId = 1},
                new Product() { Name = "Áo bóng đá nike", Quantity = 2000, Url = "https://product.hstatic.net/200000174405/product/ao-bong-da-nike-nam-cj9917-451-1_8d852b157bd14a90ab81f12c8a7f25ed_b7c393cef7d6483bb22e83b8b03df81e_master.jpg", Price = 200000, Description = "Rộng rãi, thoáng mát", CategoryId = 2},
                new Product() { Name = "Áo bóng rổ nike", Quantity = 2000, Url = "https://product.hstatic.net/200000174405/product/dv9968-010-1_a6c2e629451a4bc2a8ffa37517d11de0_master.jpg", Price = 200000, Description = "Áo bóng rổ nike AS M NK DF ICON+ JERSEY nam DV9968-010 mang lại tính chuyển động nhẹ cho cuộc đua của bạn.", CategoryId = 2},
                new Product() { Name = "Găng tay thể thao Adidas", Quantity = 2000, Url = "https://product.hstatic.net/200000174405/product/gb12424-1_2a98b120ad6747eca4bd9c1804f0f693_1024x1024.jpg", Price = 100000, Description = "", CategoryId = 3},
                new Product() { Name = "THẢM THỂ DỤC Adidas", Quantity = 2000, Url = "https://product.hstatic.net/200000174405/product/tham_the_duc_adidas_admt-11014rd_1048b2a9c0464781a21dc41f201275a2_92f343e2adca42618dd4c0771eea1bc5_1024x1024.jpg", Price = 300000, Description = "Chất liệu bền", CategoryId = 3},
                new Product() { Name = "Dép sportswear nike", Quantity = 2000, Url = "https://product.hstatic.net/200000174405/product/1_2da00232abd64548b49909241218a81f_master.jpg", Price = 100000, Description = "SỰ THOẢI MÁI KHÔNG THỂ CHỐI TỪ. SỰ LINH HOẠT VÔ HẠN.", CategoryId = 4},
                new Product() { Name = "Dép thể thao adidas", Quantity = 2000, Url = "https://product.hstatic.net/200000174405/product/f35538-1_9d20486f06094ef5a63a069de2968353_master.jpg", Price = 150000, Description = " Lớp đệm mềm thư giãn đôi chân mệt mỏi bằng sự thoải mái sang trọng.", CategoryId = 4},
                new Product() { Name = "Giày running Nike Air Zoom Pegasus", Quantity = 2000, Url = "https://product.hstatic.net/200000174405/product/1_9b15f268cf8b42e893cb47874c963fc1_master.jpg", Price = 200000, Description = "Lớp đệm thêm ở lưỡi gà và cổ giúp giữ cho đôi chân của bạn luôn cảm thấy thoải mái. Chúng tôi cũng đặt lưới được chế tạo - chắc chắn hơn và linh hoạt hơn lưới thông thường - ở trên cùng giúp tăng khả năng thông thoáng.", CategoryId = 5},
                new Product() { Name = "Giày running adidas Runfalcon", Quantity = 2000, Url = "https://product.hstatic.net/200000174405/product/gv9571-1_5c28f08ae2fd4c19ab9541af8c4d7f09_master.jpg", Price = 200000, Description = "Khi mang đôi giày adidas này, bạn sẵn sàng chạy một vòng công viên rồi sau đó hẹn hò cafe cùng hội bạn. ", CategoryId = 5},
                new Product() { Name = "Áo Polo Nữ AM màu đen", Quantity = 2000, Url = "https://product.hstatic.net/200000174405/product/tra00153_6939f994087743ac9beaa7fc3af6b22e_master.jpg", Price = 250000, Description = "Hàng chất lượng cao", CategoryId = 6},
                new Product() { Name = "Áo Polo nữ AM màu trắng", Quantity = 2000, Url = "https://product.hstatic.net/200000174405/product/rew02421_1aeb8967c0a849a28e23d6cd8673ff40_master.jpg", Price = 250000, Description = "Hàng chất lượng cao", CategoryId = 6},
                new Product() { Name = "Cước tennis PRO XTREME ", Quantity = 2000, Url = "https://product.hstatic.net/200000174405/product/cuoc_tennis_pro_xtreme_200m_16b0db1fb6e649b29a1968e38c04e4a7_1024x1024.jpg", Price = 100000, Description = "Chất liệu bền", CategoryId = 7},
                new Product() { Name = "Quấn cán PRO TACKY", Quantity = 2000, Url = "https://product.hstatic.net/200000174405/product/quan_can_pro_tacky_x_3_653039_d2acfcd7c50a48b4ab3dfa8916af534b_1024x1024.jpg", Price = 150000, Description = "Hàng giảm giá", CategoryId = 7},
                new Product() { Name = "Dép sportswear NIKE VICTORI ONE", Quantity = 2000, Url = "https://product.hstatic.net/200000174405/product/cn9677-007-1_01d6f2de5aaa43d2934103705bd4bc69_master.jpg", Price = 100000, Description = "Khuyến mại Nike", CategoryId = 8},
                new Product() { Name = "Dép sportswear Nike On Deck", Quantity = 2000, Url = "https://product.hstatic.net/200000174405/product/1_e66ac201f2374064b357166a79ba22bb_master.jpg", Price = 150000, Description = "Đế dép được thiết kế theo phong cách Waffle cổ điển giúp tăng cường độ đàn hồi, độ bám và độ bền.", CategoryId = 8},

            };
                context.Products.AddRange(products);
                context.SaveChanges();
            }

            // Look for any product size.
            if (!context.ProductSizes.Any())
            {
                List<ProductSize> productSizes = new()
            {
                new ProductSize() { Name = "M", Price = 200000, ProductId = 1 },
                new ProductSize() { Name = "M", Price = 150000, ProductId = 2 },
                new ProductSize() { Name = "M", Price = 200000, ProductId = 3 },
                new ProductSize() { Name = "M", Price = 200000, ProductId = 4 },
                new ProductSize() { Name = "M", Price = 100000, ProductId = 5 },
                new ProductSize() { Name = "M", Price = 300000, ProductId = 6 },
                new ProductSize() { Name = "M", Price = 100000, ProductId = 7 },
                new ProductSize() { Name = "M", Price = 150000, ProductId = 8 },
                new ProductSize() { Name = "M", Price = 200000, ProductId = 9 },
                new ProductSize() { Name = "M", Price = 200000, ProductId = 10 },
                new ProductSize() { Name = "M", Price = 250000, ProductId = 11 },
                new ProductSize() { Name = "M", Price = 250000, ProductId = 12 },
                new ProductSize() { Name = "M", Price = 100000, ProductId = 13 },
                new ProductSize() { Name = "M", Price = 150000, ProductId = 14 },
                new ProductSize() { Name = "M", Price = 100000, ProductId = 15 },
                new ProductSize() { Name = "M", Price = 150000, ProductId = 16 },
                new ProductSize() { Name = "S", Price = 100000, ProductId = 2 },
                new ProductSize() { Name = "L", Price = 200000, ProductId = 2 },
                new ProductSize() { Name = "S", Price = 150000, ProductId = 1 },
                new ProductSize() { Name = "L", Price = 250000, ProductId = 1 },
            };
                context.ProductSizes.AddRange(productSizes);
                context.SaveChanges();
            }

        }
    }
}
