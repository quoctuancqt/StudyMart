using StudyMart.Web.Services;

namespace StudyMart.Web.Extensions;

public static class ServiceExtensions
{
    public static TBuilder AddAppServices<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddScoped<CategoryService>();
        builder.Services.AddScoped<ProductService>();
        return builder;
    }
}