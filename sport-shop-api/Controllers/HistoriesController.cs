using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sport_shop_api.Data;
using sport_shop_api.Models.DTOs;
using sport_shop_api.Models.Entities;
using System.Security.Claims;

namespace sport_shop_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class HistoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public HistoriesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }

        // GET: api/Histories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<History>>> GetHistories()
        {
            string role = GetCurrentRole();
            if (role == "Admin")
            {
                var list = await _context.Histories.Include(h => h.User)
                    .Include(h => h.HistoryProducts)
                    .ThenInclude(hp => hp.ProductSize)
                    .ThenInclude(pz => pz.Product)
                    .Select(h => new
                    {
                        h.Id,
                        h.OnDelivery,
                        h.CreatedDate,
                        h.UpdatedDate,
                        h.UserId,
                        h.User.Address,
                        h.HistoryProducts
                    }).ToListAsync();
                return Ok(list);
            }
            else
            {
                int userId = Int32.Parse(GetCurrentUserId());
                var list = await _context.Histories.Include(h => h.User)
                    .Where(h => h.User.UserId == userId)
                    .Include(h => h.HistoryProducts)
                    .ThenInclude(hp => hp.ProductSize)
                    .ThenInclude(pz => pz.Product)
                    .Select(h => new
                    {
                        h.Id,
                        h.OnDelivery,
                        h.CreatedDate,
                        h.UpdatedDate,
                        h.UserId,
                        h.User.Address,
                        h.HistoryProducts
                    }).ToListAsync();
                return Ok(list);
            }
        }

        // PUT: api/Histories/5
        [HttpPut("{id}"), Authorize(Roles = "Delivery")]
        public async Task<IActionResult> PutHistory(int id)
        {
            History history = await _context.Histories.FirstOrDefaultAsync(h => h.Id == id);


            if (history == null)
            {
                return NotFound();
            }

            if (history.OnDelivery) return BadRequest();

            history.OnDelivery = true;
            history.UpdatedDate = DateTime.UtcNow;
            _context.Entry(history).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return Ok();
        }

        // POST: api/Histories
        [HttpPost]
        public async Task<ActionResult<History>> PostHistory(List<HistoryProductDTO> historyProductDTOs)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                historyProductDTOs.ForEach(hp =>
                {
                    bool existed = _context.ProductSizes.Any(ps => ps.Id == hp.ProductSizeId);
                    if (!existed || hp.Quantity <= 0) throw new Exception("Invalid product or quantity");
                });

                History newHistory = new()
                {
                    UserId = int.Parse(GetCurrentUserId())
                };
                _context.Histories.Add(newHistory);
                await _context.SaveChangesAsync();

                List<HistoryProduct> historyProducts = _mapper.Map<List<HistoryProduct>>(historyProductDTOs);

                historyProducts.ForEach(hp => hp.HistoryId = newHistory.Id);

                _context.HistoryProducts.AddRange(historyProducts);
                await _context.SaveChangesAsync();

                // decrease quantity of product
                History history = await _context.Histories.Include(h => h.HistoryProducts)
                .ThenInclude(hp => hp.ProductSize)
                .ThenInclude(pz => pz.Product)
                .FirstOrDefaultAsync(h => h.Id == newHistory.Id);

                history.HistoryProducts.ForEach(hp =>
                {
                    hp.ProductSize.Product.Quantity -= hp.Quantity;
                    _context.Entry(hp.ProductSize.Product);
                });
                await _context.SaveChangesAsync();
                transaction.Commit();
                return Ok(new { newHistory.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            };
        }

        // DELETE: api/Histories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistory(int id)
        {
            var history = await _context.Histories.FindAsync(id);
            if (history == null)
            {
                return NotFound();
            }

            if (history.OnDelivery) return BadRequest();

            _context.Histories.Remove(history);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private string GetCurrentUserId()
        {
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return userClaims.FirstOrDefault(o => o.Type == "userId")?.Value;
            }
            return null;
        }

        private string GetCurrentRole()
        {
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
            }
            return null;
        }
    }
}
