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
            try
            {
                Product oldProduct = await _context.Products.FindAsync(id);

                if (newProductDTO.File?.Length > 0)
                {
                    string stroredPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot/files"));

                    if (oldProduct.Url.Contains("azurewebsites"))
                    {
                        string imgPath = oldProduct.Url[47..];
                        string fullPath = Path.Combine(stroredPath, imgPath);
                        System.IO.File.Delete(fullPath);
                    }

                    string newImgPath = DateTime.Now.ToString("yyyyMMddTHHmmss") + newProductDTO.File.FileName;
                    string newFullPath = Path.Combine(stroredPath, newImgPath);
                    using (var fileStream = new FileStream(newFullPath, FileMode.Create))
                    {
                        await newProductDTO.File.CopyToAsync(fileStream);
                    }
                    string Url = $@"https://sport-shop-api.azurewebsites.net/files/{newImgPath}";
                    oldProduct.Url = Url;

                }
                oldProduct.Name = newProductDTO.Name;
                oldProduct.Description = newProductDTO.Description;
                oldProduct.Quantity = newProductDTO.Quantity;
                oldProduct.Price = newProductDTO.Price;
                oldProduct.CategoryId = newProductDTO.CategoryId;

                _context.Entry(oldProduct).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok(new { oldProduct.Url });
            }
            catch (Exception)
            {
                return BadRequest("dasd");
            }
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromForm] ProductFileDTO productDTO)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDTO);
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

                    product.Url = Url;
                }
                product.Sizes = new List<ProductSize>
                {
                    new() { Name = "S", Price = product.Price },
                    new() { Name = "M", Price = product.Price },
                    new() { Name = "L", Price = product.Price },
                };
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return Ok(new { product.Url, product.Id });
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
            if (product.Url.Contains("azurewebsites"))
            {
                string stroredPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot/files"));
                string imgPath = product.Url[47..];
                string fullPath = Path.Combine(stroredPath, imgPath);
                System.IO.File.Delete(fullPath);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
