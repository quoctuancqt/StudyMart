using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using StudyMart.Web;
using StudyMart.Web.Components;
using StudyMart.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor()
    .AddTransient<AuthorizationHandler>();

builder.Services.AddHttpLogging();

builder.AddApiClient();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(b =>
    {
        b.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddKeycloakOpenIdConnect(
        "keycloak",
        realm: "study-mart",
        options =>
        {
            options.ClientId = "web";
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.Scope.Add("offline_access");
            options.Scope.Add("profile");
            options.Scope.Add("openid");
            options.RequireHttpsMetadata = false;
            options.SaveTokens = true;
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.UsePkce = true;
        })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Events = new CookieAuthenticationEvents
        {
            // After the auth cookie has been validated, this event is called.
            // In it we see if the access token is close to expiring.  If it is
            // then we use the refresh token to get a new access token and save them.
            // If the refresh token does not work for some reason then we redirect to 
            // the login screen.
            OnValidatePrincipal = async cookieCtx =>
            {
                var now = DateTimeOffset.UtcNow;
                var expiresAt = cookieCtx.Properties.GetTokenValue("expires_at");
                var accessTokenExpiration = DateTimeOffset.Parse(expiresAt!);
                var timeRemaining = accessTokenExpiration.Subtract(now);
                // TODO: Get this from configuration with a fall back value.
                var refreshThresholdMinutes = 1440;
                var refreshThreshold = TimeSpan.FromMinutes(refreshThresholdMinutes);

                if (timeRemaining < refreshThreshold)
                {
                    var refreshToken = cookieCtx.Properties.GetTokenValue("refresh_token");
                    // TODO: Get this HttpClient from a factory
                    var httpClientFactory = cookieCtx.HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>();
                    var client = httpClientFactory.CreateClient();
                    var response = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
                    {
                        Address = "http://localhost:8080/realms/study-mart/protocol/openid-connect/token",
                        ClientId = "web",
                        RefreshToken = refreshToken!
                    });

                    if (!response.IsError)
                    {
                        var expiresInSeconds = response.ExpiresIn;
                        var updatedExpiresAt = DateTimeOffset.UtcNow.AddSeconds(expiresInSeconds);
                        cookieCtx.Properties.UpdateTokenValue("expires_at", updatedExpiresAt.ToString());
                        cookieCtx.Properties.UpdateTokenValue("access_token", response.AccessToken!);
                        cookieCtx.Properties.UpdateTokenValue("refresh_token", response.RefreshToken!);
                        
                        // Indicate to the cookie middleware that the cookie should be remade (since we have updated it)
                        cookieCtx.ShouldRenew = true;
                    }
                    else
                    {
                        cookieCtx.RejectPrincipal();
                        await cookieCtx.HttpContext.SignOutAsync();
                    }
                }
            }
        };
    });

builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorization();

builder.AddAppServices();
builder.Services.AddBlazorBootstrap();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseOutputCache();

app.UseCookiePolicy();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.MapGroup("/authentication").MapLoginAndLogout();

app.Run();
