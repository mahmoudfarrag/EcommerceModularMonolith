using Microsoft.AspNetCore.Mvc;
using Orders.Application.Services;

namespace Orders.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly OrderService _service;
    public OrderController(OrderService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpPost("{productId:guid}")]
    public async Task<IActionResult> PlaceOrder(Guid productId)
    {
        var id = await _service.PlaceOrderAsync(productId);
        return Ok(new { OrderId = id });
    }
}
