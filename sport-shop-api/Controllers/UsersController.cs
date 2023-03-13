﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User userLogin)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == userLogin.Email.ToLower());

            bool verified = BC.Verify(userLogin.Password, currentUser.Password);

            if (verified)
            {
                var token = await GenerateToken(currentUser);
                return Ok(new
                {
                    msg = "Login successfully",
                    data = token
                });
            }
            return NotFound(new
            {
                msg = "User is not found"
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User userRegister)
        {
            var existed = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == userRegister.Email.ToLower());

            if (existed == null)
            {
                userRegister.Password = BC.HashPassword(userRegister.Password);
                _context.Users.Add(userRegister);
                await _context.SaveChangesAsync();
                TokenDTO token = await GenerateToken(userRegister);
                return Ok(new
                {
                    msg = "Register successfully",
                    data = token
                });

            }

            return BadRequest(new
            {
                msg = "User is existed"
            });
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
                    int userId = int.Parse(tokenVerification.Claims.FirstOrDefault(r => r.Type == "UserId").Value);

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
                            return Ok(new
                            {
                                msg = "Renew token successfully",
                                data = newToken
                            });
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
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    msg = ex.Message
                });
            }


        }

        private async Task<TokenDTO> GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Identity:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.UserId.ToString()),
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
                refresh_token = refreshToken
            };
        }
    }
}

