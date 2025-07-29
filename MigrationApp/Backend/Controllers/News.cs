
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
    public class NewsController : ControllerBase
    {
        private readonly BackendDbContext _context;

        public NewsController(BackendDbContext context)
        {
            _context = context;
        }

        // GET: api/News
        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            var newsList = await _context.News.OrderByDescending(x => x.NewsId).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return Ok(newsList);
        }

        // GET: api/News/5
        [HttpGet("{id}")]
        public async Task<ActionResult<News>> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return Ok(news);
        }

        // POST: api/News
        [HttpPost]
        public async Task<ActionResult<News>> Create([FromBody] News news)
        {
            if (ModelState.IsValid)
            {
                _context.News.Add(news);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Details), new { id = news.NewsId }, news);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/News/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] News news)
        {
            if (id != news.NewsId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Entry(news).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.News.Any(e => e.NewsId == id))
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

        // DELETE: api/News/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
