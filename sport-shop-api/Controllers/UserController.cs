using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sport_shop_api.Models;
using System.Security.Claims;

namespace sport_shop_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return Ok($"Hi");
        }


        [HttpGet("Public")]
        public IActionResult Public()
        {
            return Ok("Hi");
        }
        public User? GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new User
                {
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }

    }

}
