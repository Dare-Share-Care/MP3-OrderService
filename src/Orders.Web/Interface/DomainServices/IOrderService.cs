using Orders.Web.Model.Dto;

namespace Orders.Web.Interface.DomainServices;

public interface IOrderService
{
    Task<OrderDto> GetOrderByIdAsync(int id);
    Task<List<OrderDto>> GetOrdersAsync();
    Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);
}