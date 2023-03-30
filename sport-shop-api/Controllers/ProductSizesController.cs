using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sport_shop_api.Data;
using sport_shop_api.Models.DTOs;
using sport_shop_api.Models.Entities;
using System.Data;

namespace sport_shop_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize(Roles = "Admin")]
    public class ProductSizesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductSizesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }

        // GET: api/ProductSizes
        [HttpGet("{id}"), AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductSize>>> GetProductSizes(int id)
        {
            List<ProductSize> productSizes = await _context.ProductSizes.Where(pz => pz.ProductId == id).ToListAsync();

            return Ok(productSizes);
        }

        // PUT: api/ProductSizes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutProductSize(int id, List<ProductSize> productSizes)
        {

            _context.ProductSizes.UpdateRange(productSizes);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductSizeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductSizes
        [HttpPost("{id}")]
        public async Task<ActionResult<ProductSize>> PostProductSize(int id, List<ProductSizeDTO> productSizeDTOs)
        {
            List<ProductSize> productSizes = _mapper.Map<List<ProductSize>>(productSizeDTOs);
            productSizes.ForEach(pz => pz.ProductId = id);
            _context.ProductSizes.AddRange(productSizes);
            await _context.SaveChangesAsync();
            return Ok(new { productSizes });
        }

        // DELETE: api/ProductSizes/5
        [HttpDelete("{productSizeId}")]
        public async Task<IActionResult> DeleteProductSize(int productSizeId)
        {
            var productSize = await _context.ProductSizes.FindAsync(productSizeId);
            if (productSize == null)
            {
                return NotFound();
            }

            _context.ProductSizes.Remove(productSize);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductSizeExists(int id)
        {
            return _context.ProductSizes.Any(e => e.Id == id);
        }
    }
}
