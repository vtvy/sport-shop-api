using EmailValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using sport_shop_api.Data;
using sport_shop_api.Models.DTOs;
using sport_shop_api.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

        [HttpPut("changeinfo/{id}")]
        public async Task<IActionResult> Info(int id, UserDTO user)
        {
            var currentUser = await _context.Users.FindAsync(id);
            if (currentUser != null)
            {

                bool verified = BC.Verify(user.Password, currentUser.Password);

                if (verified)
                {
                    currentUser.Name = user.Name;
                    currentUser.Email = user.Email;
                    currentUser.Address = user.Address;
                    _context.Entry(currentUser).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            return Unauthorized();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO userLogin)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == userLogin.Email.ToLower());
            if (currentUser != null)
            {

                bool verified = BC.Verify(userLogin.Password, currentUser.Password);

                if (verified)
                {
                    TokenDTO token = await GenerateToken(currentUser);
                    return Ok(token);
                }
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userRegister)
        {
            if (!EmailValidator.Validate(userRegister.Email)) return BadRequest("Invalid email");

            var existed = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == userRegister.Email.ToLower());

            if (existed == null)
            {
                User newUser = new()
                {
                    Email = userRegister.Email,
                    Password = BC.HashPassword(userRegister.Password)
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                TokenDTO token = await GenerateToken(newUser);
                return Ok(token);

            }

            return Unauthorized();
        }

        [HttpPost("renewtoken")]
        public async Task<IActionResult> RenewToken(TokenDTO oldToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Identity:Key"]));

            try
            {


                var tokenVerification = jwtTokenHandler.ValidateToken(oldToken.access_token,
                   new TokenValidationParameters
                   {
                       ValidateLifetime = false,
                       ValidateIssuer = false,
                       ValidateAudience = false,

                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Identity:Key"])),
                       ClockSkew = TimeSpan.Zero
                   },
                out var validdatedToken);

                // if valid access token and algorithm
                if (validdatedToken is JwtSecurityToken jwtSecurityToken && jwtSecurityToken.Header.Alg.Equals
                        (SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
                {
                    int userId = int.Parse(tokenVerification.Claims.FirstOrDefault(r => r.Type == "userId")?.Value);

                    RefreshToken storedRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync
                        (t => t.Token == oldToken.refresh_token);

                    // if refresh token is exist, not used and not expired
                    if (storedRefreshToken?.IsUsed == false
                        && storedRefreshToken.ExpiredAt >= DateTime.Now)
                    {
                        // if userId from refresh token and access token is the same
                        // pass all validations
                        if (storedRefreshToken.UserId == userId)
                        {
                            storedRefreshToken.IsUsed = true;
                            _context.RefreshTokens.Update(storedRefreshToken);
                            await _context.SaveChangesAsync();


                            User user = _context.Users.FirstOrDefault(u => u.UserId == userId);
                            TokenDTO newToken = await GenerateToken(user);
                            return Ok(newToken);
                        }
                        else
                        {
                            throw new Exception("Invalid access token and refresh token");
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid refresh token");
                    }
                }
                else
                {
                    throw new Exception("Invalid access token");
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }


        }

        private async Task<TokenDTO> GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Identity:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            var claims = new[]
            {
                new Claim("email", user.Email),
                new Claim("name", user.Name),
                new Claim("role", user.Role),
                new Claim("userId", user.UserId.ToString()),
            };

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddHours(1), signingCredentials: credentials);

            string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var random = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(random);

            string refreshToken = Convert.ToBase64String(random);

            // store it to database
            RefreshToken refreshTokenEntity = new()
            {
                UserId = user.UserId,
                Token = refreshToken,
                IsUsed = false,
                ExpiredAt = DateTime.Now.AddDays(1)
            };

            await _context.RefreshTokens.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new TokenDTO
            {
                access_token = accessToken,
                refresh_token = refreshToken,
            };
        }
    }
}

