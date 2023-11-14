using Orders.Web.Model.Dto.CreateOrder;

namespace Orders.Web.Exceptions;

public class InvalidQuantitiesException : Exception
{
    public InvalidQuantitiesException(List<CreateOrderLineDto> dto) : base($"Invalid quantities: {string.Join(", ", dto.Select(x => x.Quantity))}")
    {
        
    }
}