using ChienVHShopOnline.DTOs;
using ChienVHShopOnline.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChienVHShopOnline.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShoppingCartController : ControllerBase
{
    private readonly IShoppingCartService _shoppingCartService;

    public ShoppingCartController(IShoppingCartService shoppingCartService)
    {
        _shoppingCartService = shoppingCartService;
    }

    [HttpPost("add")]
    public ActionResult AddToCart([FromBody] CartItemDto item)
    {
        // For now, we'll just return OK since the cart is managed on the frontend
        // In a real application, you might want to store cart items in the database
        return Ok(new { message = "Item added to cart successfully" });
    }

    [HttpPost("checkout")]
    [Authorize]
    public async Task<ActionResult<OrderResponseDto>> Checkout([FromBody] OrderRequestDto request)
    {
        var result = await _shoppingCartService.ProcessOrderAsync(request);
        return Ok(result);
    }
}
