using Orders.Web.Model.Dto;

namespace Orders.Web.Interface.DomainServices;

public interface ICatalogueService
{
    Task<List<ProductDto>> GetCatalogAsync();
}