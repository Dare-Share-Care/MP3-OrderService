namespace Orders.Web.Entities;

public class Order : BaseEntity
{
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderLine> OrderLines { get; set; } = new();
}