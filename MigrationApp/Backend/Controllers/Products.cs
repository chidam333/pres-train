using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly BackendDbContext _context;

        public ProductsController(BackendDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Index(int? page, int? category)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;
            
            var query = _context.Products.OrderByDescending(x => x.ProductId).AsQueryable();

            if (category != null)
            {
                query = query.Where(x => x.CategoryId == category);
            }

            var productList = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return Ok(productList);
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var productList = await _context.Products.OrderByDescending(x => x.ProductId).ToListAsync();
            return Ok(productList);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> Search([FromQuery] string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return Ok(new List<Product>());
            }
            var productList = await _context.Products
                                        .Where(p => p.ProductName.Contains(q))
                                        .OrderByDescending(x => x.ProductId)
                                        .ToListAsync();
            return Ok(productList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Details(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create([FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Details), new { id = product.ProductId }, product);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(product).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.ProductId == id))
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
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
