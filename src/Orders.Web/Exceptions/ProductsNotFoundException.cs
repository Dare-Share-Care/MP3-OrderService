namespace Orders.Web.Exceptions;

public class ProductsNotFoundException : Exception
{
    public ProductsNotFoundException(List<int> ids) : base($"Products with id {ids} was not found")
    {
    }
}