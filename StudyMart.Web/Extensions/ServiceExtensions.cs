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
        
        builder.Services.AddScoped<CartContainer>();

        return builder;
    }
}