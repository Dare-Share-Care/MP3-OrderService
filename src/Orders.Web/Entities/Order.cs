namespace Orders.Web.Entities;

public class Order : BaseEntity
{
    public DateTime OrderDate { get; set; }
    public List<OrderLine> OrderLines { get; set; } = new();
}