using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly BackendDbContext _context;

        public OrderController(BackendDbContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            var orderList = await _context.Orders.OrderByDescending(x => x.OrderID).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return Ok(orderList);
        }

        // GET: api/Order/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            var orderList = await _context.Orders.OrderByDescending(x => x.OrderID).ToListAsync();
            return Ok(orderList);
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
    }
}