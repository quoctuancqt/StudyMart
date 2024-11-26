using System.Net.Mail;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Scalar.AspNetCore;
using StudyMart.ApiService.Data;
using StudyMart.ApiService.Features.Category;
using StudyMart.MailKit.Client;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<AppDbContext>("postgresqldb");

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add services to the container.
builder.AddMailKitClient("maildev");

builder.Services.AddAuthentication()
    .AddKeycloakJwtBearer("keycloak", realm: "WeatherShop", options =>
    {
        options.RequireHttpsMetadata = false;
        options.Audience = "weather.api";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.AddServer(builder.Configuration.GetValue("SCALAR_SERVER_URL", "https://localhost:5001"));
        options.WithTheme(ScalarTheme.BluePlanet);
        options.Title = "Weather API";
        options.WithDarkModeToggle(true);
    });

    var scope = app.Services.CreateAsyncScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapCategories();

// app.MapPost("/subscribe",
//     async (MailKitClientFactory factory, string email) =>
//     {
//         ISmtpClient client = await factory.GetSmtpClientAsync();

//         using var message = new MailMessage("newsletter@yourcompany.com", email);
//         message.Subject = "Welcome to our newsletter!";
//         message.Body = "Thank you for subscribing to our newsletter!";

//         await client.SendAsync(MimeMessage.CreateFromMailMessage(message));
//     });

// app.MapPost("/unsubscribe",
//     async (MailKitClientFactory factory, string email) =>
//     {
//         ISmtpClient client = await factory.GetSmtpClientAsync();

//         using var message = new MailMessage("newsletter@yourcompany.com", email);
//         message.Subject = "You are unsubscribed from our newsletter!";
//         message.Body = "Sorry to see you go. We hope you will come back soon!";

//         await client.SendAsync(MimeMessage.CreateFromMailMessage(message));
//     });

app.MapDefaultEndpoints();

app.Run();
