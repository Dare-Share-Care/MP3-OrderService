namespace Orders.Web.Model.Dto.CreateOrder;

public class CreateOrderDto
{
    public int CustomerId { get; set; }
    public List<CreateOrderLineDto> Lines { get; set; } = new();
}