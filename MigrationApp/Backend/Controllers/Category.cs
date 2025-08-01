
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
    public class CategoryController : ControllerBase
    {
        private readonly BackendDbContext _context;

        public CategoryController(BackendDbContext context)
        {
            _context = context;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;
            var catList = await _context.Categories.OrderBy(x => x.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return Ok(catList);
        }

        // GET: api/Category/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            var catList = await _context.Categories.OrderBy(x => x.Name).ToListAsync();
            return Ok(catList);
        }

        // GET: api/Category/search?q=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Category>>> Search([FromQuery] string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return Ok(new List<Category>());
            }
            var catList = await _context.Categories
                                        .Where(c => c.Name.Contains(q))
                                        .OrderBy(x => x.Name)
                                        .ToListAsync();
            return Ok(catList);
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Details(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<Category>> Create([FromBody] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Details), new { id = category.CategoryId }, category);
            }

            return BadRequest(ModelState);
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(category).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Categories.Any(e => e.CategoryId == id))
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

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
