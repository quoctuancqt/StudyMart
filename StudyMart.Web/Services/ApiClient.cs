using System.Text.Json;
using System.Text.Json.Serialization;
using Refit;
using StudyMart.Contract.Clients;

namespace StudyMart.Web.Services;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    private static readonly RefitSettings RefitSettings = new()
    {
        ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        }),
        CollectionFormat = CollectionFormat.Multi
    };

    public ApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(nameof(ApiClient));
    }

    private ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public static ApiClient GetInstance(HttpClient httpClient)
    {
        return new ApiClient(httpClient);
    }

    private T For<T>(string clientName)
    {
        return RestService.For<T>(_httpClient, RefitSettings);
    }
    
    public ICartClient CartClient => For<ICartClient>("CartClient");
    public ICategoryClient CategoryClient => For<ICategoryClient>("CategoryClient");
    public IOrderClient OrderClient => For<IOrderClient>("OrderClient");
    public IProductClient ProductClient => For<IProductClient>("ProductClient");
}