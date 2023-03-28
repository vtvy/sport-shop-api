using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sport_shop_api.Data;
using sport_shop_api.Models.DTOs;
using sport_shop_api.Models.Entities;

namespace sport_shop_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Categories
        [HttpGet, AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            List<Category> categories = await _context.Categories.ToListAsync();
            List<CategoryDTO> categoryDTOs = _mapper.Map<List<CategoryDTO>>(categories);
            return Ok(categoryDTOs);
        }

        // PUT: api/Categories/5
        [HttpPut]
        public async Task<IActionResult> PutCategory(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                return NotFound();
            }

            return Ok();
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
