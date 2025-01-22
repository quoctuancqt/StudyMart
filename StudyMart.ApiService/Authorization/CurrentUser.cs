using System.Security.Claims;

namespace StudyMart.ApiService.Authorization;

public class CurrentUser(IHttpContextAccessor httpContextAccessor)
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;
    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    public string? UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? UserName => User?.FindFirstValue(ClaimTypes.Name);
    public bool IsAdmin => User?.IsInRole("Administrator") ?? false;
}