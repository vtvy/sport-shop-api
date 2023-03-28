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
    [ApiController, Authorize(Roles = "Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }

        // GET: api/Products
        [HttpGet, AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            List<Product> products = await _context.Products.Include(p => p.Sizes).ToListAsync();
            return Ok(products);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromForm] ProductFileDTO newProductDTO)
        {
            newProductDTO.Id = id;

            try
            {
                Product oldProduct = await _context.Products.FindAsync(newProductDTO.Id);
                Product newProduct = _mapper.Map<Product>(newProductDTO);

                if (newProductDTO.File?.Length > 0)
                {
                    string stroredPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot/files"));
                    string imgPath = oldProduct.Url[47..];
                    string fullPath = Path.Combine(stroredPath, imgPath);
                    System.IO.File.Delete(fullPath);

                    string newImgPath = DateTime.Now.ToString("yyyyMMddTHHmmss") + newProductDTO.File.FileName;
                    string newFullPath = Path.Combine(stroredPath, imgPath);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await newProductDTO.File.CopyToAsync(fileStream);
                    }
                    string Url = $@"https://sport-shop-api.azurewebsites.net/files/{newImgPath}";
                    newProduct.Url = Url;
                    _context.Entry(newProduct).State = EntityState.Detached;
                    await _context.SaveChangesAsync();
                    return Ok(new { Url });
                }
                else
                {
                    newProduct.Url = oldProduct.Url;
                    _context.Entry(newProduct).State = EntityState.Detached;
                    await _context.SaveChangesAsync();
                    return Ok();
                }

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromForm] ProductFileDTO productDTO)
        {
            try
            {
                if (productDTO.File?.Length > 0)
                {
                    string stroredPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot/files"));
                    if (!Directory.Exists(stroredPath))
                    {
                        Directory.CreateDirectory(stroredPath);
                    }
                    string imgPath = DateTime.Now.ToString("yyyyMMddTHHmmss") + productDTO.File.FileName;
                    string fullPath = Path.Combine(stroredPath, imgPath);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await productDTO.File.CopyToAsync(fileStream);
                    }
                    string Url = $@"https://sport-shop-api.azurewebsites.net/files/{imgPath}";

                    Product product = _mapper.Map<Product>(productDTO);
                    product.Url = Url;
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();

                    return Ok(new { Url });
                }

                return BadRequest("File not found");
            }

            catch (Exception)
            {
                return BadRequest();
            }

        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            string stroredPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot/files"));
            string imgPath = product.Url[47..];
            string fullPath = Path.Combine(stroredPath, imgPath);
            System.IO.File.Delete(fullPath);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
