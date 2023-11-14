using Orders.Web.Entities;
using Orders.Web.Exceptions;
using Orders.Web.Interface.DomainServices;
using Orders.Web.Interface.Repositories;
using Orders.Web.Model.Dto;
using Orders.Web.Specifications;

namespace Orders.Web.Service;

public class OrderService : IOrderService
{
    private readonly IReadRepository<Order> _orderReadRepository;
    private readonly IRepository<Order> _orderRepository;

    public OrderService(IReadRepository<Order> orderReadRepository, IRepository<Order> orderRepository)
    {
        _orderReadRepository = orderReadRepository;
        _orderRepository = orderRepository;
    }


    public async Task<OrderDto> GetOrderByIdAsync(int id)
    {
        //Get order from repository
        var order = await _orderReadRepository.FirstOrDefaultAsync(new OrderAndOrderLinesSpec(id));

        //Order doesn't exist
        if (order == null) throw new OrderNotFoundException(id);

        //Order exists, map to DTO
        var orderDto = new OrderDto
        {
            Id = order.Id,
            Created = order.OrderDate,
            Lines = order.OrderLines.Select(orderLine => new OrderLineDto
            {
                Id = orderLine.Id,
                ProductId = orderLine.ProductId,
                Quantity = orderLine.Quantity
            }).ToList()
        };
        return orderDto;
    }

    public async Task<List<OrderDto>> GetOrdersAsync()
    {
        //Get orders from repository
        var orders = await _orderReadRepository.ListAsync(new OrdersAndOrderLinesSpec());

        //Map to list<dto>
        var orderDtos = orders.Select(order => new OrderDto
        {
            Id = order.Id,
            Created = order.OrderDate,
            Lines = order.OrderLines.Select(orderLine => new OrderLineDto
            {
                Id = orderLine.Id,
                ProductId = orderLine.ProductId,
                Quantity = orderLine.Quantity
            }).ToList()
        }).ToList();

        return orderDtos;
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto orderDto)
    {
        throw new NotImplementedException();
    }
}