using IdentityApp.Data;
using IdentityApp.Data.Entities;
using IdentityApp.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            IEnumerable<Product> foundProducts = await _context
                .Products
                .AsNoTracking()
                .ToListAsync();

            return Ok(foundProducts);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request) 
        {
            if (ModelState.IsValid)
            {
                Product productToCreate = new Product
                {
                    Name = request.Name,
                    Price = request.Price,
                    Category = request.Category
                };

                await _context.Products.AddAsync(productToCreate);
                await _context.SaveChangesAsync();

                return Ok(productToCreate);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            Product? foundProduct = await _context
                .Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (foundProduct is null)
            {
                return NotFound($"Product {id} not found");
            }

            _context.Products.Remove(foundProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
