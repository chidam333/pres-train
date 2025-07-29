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

        [HttpGet("details/{id}")]
        public async Task<ActionResult<Product>> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
