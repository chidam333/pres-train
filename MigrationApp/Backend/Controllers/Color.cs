
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
    public class ColorController : ControllerBase
    {
        private readonly BackendDbContext _context;

        public ColorController(BackendDbContext context)
        {
            _context = context;
        }

        // GET: api/Color
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Color>>> Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            var colorList = await _context.Colors.OrderBy(x => x.Color1).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return Ok(colorList);
        }

        // GET: api/Color/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Color>>> GetAll()
        {
            var colorList = await _context.Colors.OrderBy(x => x.Color1).ToListAsync();
            return Ok(colorList);
        }

        // GET: api/Color/search?q=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Color>>> Search([FromQuery] string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return Ok(new List<Color>());
            }
            var colorList = await _context.Colors
                                        .Where(c => c.Color1.Contains(q))
                                        .OrderBy(x => x.Color1)
                                        .ToListAsync();
            return Ok(colorList);
        }

        // GET: api/Color/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Color>> Details(int id)
        {
            var color = await _context.Colors.FindAsync(id);
            if (color == null)
            {
                return NotFound();
            }
            return Ok(color);
        }

        // POST: api/Color
        [HttpPost]
        public async Task<ActionResult<Color>> Create([FromBody] Color color)
        {
            if (ModelState.IsValid)
            {
                _context.Colors.Add(color);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Details), new { id = color.ColorId }, color);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Color/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Color color)
        {
            if (id != color.ColorId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(color).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Colors.Any(e => e.ColorId == id))
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

        // DELETE: api/Color/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var color = await _context.Colors.FindAsync(id);
            if (color == null)
            {
                return NotFound();
            }
            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
