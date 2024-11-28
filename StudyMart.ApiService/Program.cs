using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StudyMart.ApiService.Authorization;
using StudyMart.ApiService.Data;
using StudyMart.ApiService.Features.Category;
using StudyMart.ApiService.Features.Product;
using StudyMart.ApiService.Features.ShoppingCart;
using StudyMart.MailKit.Client;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<AppDbContext>("postgresqldb");

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Study Mart API",
        Version = "v1"
    });

    // Define the OAuth2.0 scheme that's in use (i.e. Implicit Flow)
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                // TO DO: Update the URL to point to your Keycloak instance.
                AuthorizationUrl = new Uri("http://localhost:8080/realms/study-mart/protocol/openid-connect/auth", UriKind.Absolute),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "Access read operations" },
                    { "profile", "Access write operations" },
                    { "offline_access", "Refresh token" }
                }
            }
        }
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            },
            ["openid", "profile", "offline_access"]
        }
    });
});

// Add services to the container.
builder.AddMailKitClient("maildev");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddKeycloakJwtBearer("keycloak", realm: "study-mart", options =>
    {
        options.RequireHttpsMetadata = false;
        options.Audience = "account";
    })
    .AddKeycloakOpenIdConnect(
        "keycloak",
        realm: "study-mart",
        options =>
        {
            options.ClientId = "scalar";
            options.Scope.Add("offline_access");
            options.Scope.Add("profile");
            options.Scope.Add("openid");
            options.RequireHttpsMetadata = false;
            options.SaveTokens = true;
        });

builder.Services.AddAuthorization();

builder.Services.AddScoped<CurrentUser>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // app.MapScalarApiReference();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Study Mart API V1");
        options.OAuthClientId("scalar");
        options.OAuthScopes("openid", "profile", "offline_access");
    });

    var scope = app.Services.CreateAsyncScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapCategories();
app.MapProductEndpoints();
app.MapShoppingCartEndpoints();

#region Mailkit Sample
// app.MapPost("/subscribe",
//     async (MailKitClientFactory factory, string email) =>
//     {
//         ISmtpClient client = await factory.GetSmtpClientAsync();
//
//         using var message = new MailMessage("newsletter@yourcompany.com", email);
//         message.Subject = "Welcome to our newsletter!";
//         message.Body = "Thank you for subscribing to our newsletter!";
//
//         await client.SendAsync(MimeMessage.CreateFromMailMessage(message));
//     });
//
// app.MapPost("/unsubscribe",
//     async (MailKitClientFactory factory, string email) =>
//     {
//         ISmtpClient client = await factory.GetSmtpClientAsync();
//
//         using var message = new MailMessage("newsletter@yourcompany.com", email);
//         message.Subject = "You are unsubscribed from our newsletter!";
//         message.Body = "Sorry to see you go. We hope you will come back soon!";
//
//         await client.SendAsync(MimeMessage.CreateFromMailMessage(message));
//     });
#endregion

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.MapDefaultEndpoints();

app.Run();