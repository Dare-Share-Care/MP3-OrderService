using Microsoft.AspNetCore.Mvc;
using Orders.Web.Interface.DomainServices;

namespace Orders.Web.Controller;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderByIdAsync(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        return Ok(order);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOrdersAsync()
    {
        var orders = await _orderService.GetOrdersAsync();
        return Ok(orders);
    }
}