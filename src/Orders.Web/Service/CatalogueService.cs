using Orders.Web.Interface.DomainServices;
using Orders.Web.Model.Dto;
using System.Text.Json;

namespace Orders.Web.Service;

public class CatalogueService : ICatalogueService
{
    private readonly HttpClient _httpClient = new();

    public async Task<List<ProductDto>> GetCatalogAsync()
    {
        const string url = "http://localhost:5289/api/Inventory";
        
        //Get products catalogue from Inventory API
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<ProductDto>>(content);
        
        return result ?? new List<ProductDto>();
    }
}