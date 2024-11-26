using System.Security.Claims;

namespace StudyMart.ApiService.Authorization;

public class CurrentUser
{
    public ClaimsPrincipal Principal { get; set; } = default!;

    public string Id => Principal.FindFirstValue(ClaimTypes.NameIdentifier)!;
    public bool IsAdmin => Principal.IsInRole("admin");
}
