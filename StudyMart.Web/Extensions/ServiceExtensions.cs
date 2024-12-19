using StudyMart.Web.Services;
using StudyMart.Web.States;

namespace StudyMart.Web.Extensions;

public static class ServiceExtensions
{
    public static TBuilder AddAppServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddLogging();
        builder.Services.AddScoped<CategoryService>();
        builder.Services.AddScoped<ProductService>();
        builder.Services.AddScoped<CartService>();
        builder.Services.AddScoped<OrderService>();
        
        builder.Services.AddScoped<CartContainer>();

        return builder;
    }
    
    public static TBuilder AddApiClient<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddHttpClient(nameof(ApiClient), client =>
            {
                // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
                // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
                client.BaseAddress = new Uri("https+http://apiservice");
            })
            .AddHttpMessageHandler<AuthorizationHandler>();
        builder.Services.AddScoped<ApiClient>();

        return builder;
    }
}