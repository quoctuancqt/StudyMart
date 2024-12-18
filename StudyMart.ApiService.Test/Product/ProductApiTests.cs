using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using StudyMart.Contract.Product;
using Xunit;

namespace StudyMart.ApiService.Test.Product;

public sealed class ProductApiTests : IClassFixture<ApiFixture>
{
    private readonly WebApplicationFactory<Program> _webApplicationFactory;
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public ProductApiTests(ApiFixture fixture)
    {
        _webApplicationFactory = fixture;
        _httpClient = _webApplicationFactory.CreateDefaultClient();
    }
    
    [Fact]
    public async Task GetProduct()
    {
        // Act
        var response = await _httpClient.GetAsync("/api/products");

        // Assert
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<ProductDto>>(body, _jsonSerializerOptions);

        Assert.NotNull(result);
        Assert.Equal(5, result.Count);
    }
}