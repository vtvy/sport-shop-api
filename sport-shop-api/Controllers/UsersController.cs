using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using sport_shop_api.Data;
using sport_shop_api.Models.Dtos;
using sport_shop_api.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace sport_shop_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private IConfiguration _configuration;
        private readonly AppDbContext _context;

        public UsersController(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO userLogin)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == userLogin.Email.ToLower());

            bool verified = BC.Verify(userLogin.Password, currentUser.Password);

            if (verified)
            {
                var token = Generate(currentUser);
                return Ok(token);
            }
            return NotFound("User is not found");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userRegister)
        {
            var existed = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == userRegister.Email.ToLower());

            if (existed == null)
            {
                User newUser = new User()
                {
                    Email = userRegister.Email,
                    Password = BC.HashPassword(userRegister.Password),
                    Role = "User"
                };

                await _context.Users.AddAsync(newUser);
                _context.SaveChanges();
                var token = Generate(newUser);
                return Ok(token);
            }

            return NotFound("User is existed");
        }

        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Identity:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.UserId.ToString())
            };

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(100), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

