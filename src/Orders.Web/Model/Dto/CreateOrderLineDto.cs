namespace Orders.Web.Model.Dto;

public class CreateOrderLineDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}