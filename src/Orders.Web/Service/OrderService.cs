using Orders.Web.Entities;
using Orders.Web.Exceptions;
using Orders.Web.Interface.DomainServices;
using Orders.Web.Interface.Repositories;
using Orders.Web.Model.Dto;
using Orders.Web.Model.Dto.CreateOrder;
using Orders.Web.Producer;
using Orders.Web.Specifications;

namespace Orders.Web.Service;

public class OrderService : IOrderService
{
    private readonly IReadRepository<Order> _orderReadRepository;
    private readonly IRepository<Order> _orderRepository;
    private readonly ICatalogueService _catalogueService;
    private readonly KafkaProducer _kafkaProducer;

    public OrderService(IReadRepository<Order> orderReadRepository, IRepository<Order> orderRepository,
        ICatalogueService catalogueService, KafkaProducer kafkaProducer)
    {
        _orderReadRepository = orderReadRepository;
        _orderRepository = orderRepository;
        _catalogueService = catalogueService;
        _kafkaProducer = kafkaProducer;
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
        var catalogue = await _catalogueService.GetCatalogAsync();

        //Validate products exist in catalogue
        var productIds = catalogue.Select(product => product.Id).ToList();
        var invalidProductIds = orderDto.Lines.Select(line => line.ProductId).Except(productIds).ToList();
        if (invalidProductIds.Any()) throw new ProductsNotFoundException(invalidProductIds);
        
        //Validate if available stock
        var invalidQuantities = orderDto.Lines.Where(line => line.Quantity <= 0).ToList();
        if (invalidQuantities.Any()) throw new InvalidQuantitiesException(invalidQuantities);

        //Create order
        var order = new Order
        {
            CustomerId = orderDto.CustomerId,
            OrderDate = DateTime.UtcNow,
            OrderLines = orderDto.Lines.Select(line => new OrderLine
            {
                ProductId = line.ProductId,
                ProductName = catalogue.Single(product => product.Id == line.ProductId).Name,
                Price = catalogue.Single(product => product.Id == line.ProductId).Price * line.Quantity,
                Quantity = line.Quantity
            }).ToList()
        };

        //Save order
        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync();

        //Map to DTO
        var orderDtoResult = new OrderDto
        {
            Id = order.Id,
            Created = order.OrderDate,
            Lines = order.OrderLines.Select(orderLine => new OrderLineDto
            {
                Id = orderLine.Id,
                ProductId = orderLine.ProductId,
                Quantity = orderLine.Quantity,
                ProductName = catalogue.Single(product => product.Id == orderLine.ProductId).Name,
                Price = catalogue.Single(product => product.Id == orderLine.ProductId).Price * orderLine.Quantity
            }).ToList()
        };
        
        //Send order created event
        await SendOrderCreatedEventAsync(orderDtoResult);

        //Return order
        return orderDtoResult;
    }

    private async Task SendOrderCreatedEventAsync(OrderDto orderDto)
    {
        await _kafkaProducer.ProduceAsync("mp3-create-order", orderDto);
    }
}