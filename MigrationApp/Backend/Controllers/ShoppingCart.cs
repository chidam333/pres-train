
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Backend.Controllers
{
    public class Cart
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public Cart(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly BackendDbContext _context;
        private const string CartSessionKey = "Cart";

        public ShoppingCartController(BackendDbContext context)
        {
            _context = context;
        }

        private static void SetObjectAsJson(ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        private static T GetObjectFromJson<T>(ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var cart = GetObjectFromJson<List<Cart>>(HttpContext.Session, CartSessionKey) ?? new List<Cart>();
            return Ok(cart);
        }

        [HttpPost("OrderNow/{id}")]
        public async Task<IActionResult> OrderNow(int? id)
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

            var cart = GetObjectFromJson<List<Cart>>(HttpContext.Session, CartSessionKey) ?? new List<Cart>();
            int index = IsExistingCheck(id, cart);
            if (index == -1)
            {
                cart.Add(new Cart(product, 1));
            }
            else
            {
                cart[index].Quantity++;
            }

            SetObjectAsJson(HttpContext.Session, CartSessionKey, cart);
            return Ok(cart);
        }

        private int IsExistingCheck(int? id, List<Cart> cart)
        {
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.ProductId == id) return i;
            }
            return -1;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var cart = GetObjectFromJson<List<Cart>>(HttpContext.Session, CartSessionKey);
            if (cart != null)
            {
                int check = IsExistingCheck(id, cart);
                if (check != -1)
                {
                    cart.RemoveAt(check);
                    SetObjectAsJson(HttpContext.Session, CartSessionKey, cart);
                }
            }
            return Ok(cart);
        }

        [HttpPost("UpdateCart")]
        public IActionResult UpdateCart([FromBody] Dictionary<string, int> quantities)
        {
            var cart = GetObjectFromJson<List<Cart>>(HttpContext.Session, CartSessionKey);
            if (cart != null)
            {
                foreach (var item in cart)
                {
                    if (quantities.TryGetValue(item.Product.ProductId.ToString(), out int quantity))
                    {
                        item.Quantity = quantity;
                    }
                }
                SetObjectAsJson(HttpContext.Session, CartSessionKey, cart);
            }
            return Ok(cart);
        }

        [HttpPost("ProcessOrder")]
        public async Task<IActionResult> ProcessOrder([FromBody] Order order)
        {
            var cart = GetObjectFromJson<List<Cart>>(HttpContext.Session, CartSessionKey);
            if (cart == null || !cart.Any() || !ModelState.IsValid)
            {
                return BadRequest("Cart is empty or order details are invalid.");
            }

            order.OrderDate = System.DateTime.Now;
            order.Status = "Processing";
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in cart)
            {
                var orderDetail = new OrderDetail
                {
                    OrderID = order.OrderID,
                    ProductID = item.Product.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                };
                _context.OrderDetails.Add(orderDetail);
            }
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove(CartSessionKey);

            return Ok(new { message = "Order processed successfully.", orderId = order.OrderID });
        }
    }
}
