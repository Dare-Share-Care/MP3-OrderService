namespace Orders.Web.Model.Dto.CreateOrder;

public class CreateOrderLineDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}