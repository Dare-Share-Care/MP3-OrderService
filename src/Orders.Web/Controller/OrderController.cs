using Microsoft.AspNetCore.Mvc;
using Orders.Web.Interface.DomainServices;
using Orders.Web.Model.Dto;

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
    
    [HttpGet("order/{id}")]
    public async Task<IActionResult> GetOrderByIdAsync(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        return Ok(order);
    }
    
    [HttpGet("orders")]
    public async Task<IActionResult> GetOrdersAsync()
    {
        var orders = await _orderService.GetOrdersAsync();
        return Ok(orders);
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateOrderAsync(CreateOrderDto createOrderDto)
    {
        var order = await _orderService.CreateOrderAsync(createOrderDto);
        return Ok(order);
    }
}