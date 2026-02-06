// Temporary test controller to debug authentication
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartJewelry.Web.Controllers;

public class TestAuthController : Controller
{
    private readonly ILogger<TestAuthController> _logger;

    public TestAuthController(ILogger<TestAuthController> logger)
    {
        _logger = logger;
    }

    // Public endpoint - no auth required
    public IActionResult Public()
    {
        var isAuth = User.Identity?.IsAuthenticated ?? false;
        var userName = User.Identity?.Name ?? "Not authenticated";
        var claims = User.Claims.Select(c => $"{c.Type}: {c.Value}").ToList();
        
        _logger.LogInformation("TestAuth/Public - IsAuthenticated: {IsAuth}", isAuth);
        _logger.LogInformation("TestAuth/Public - UserName: {UserName}", userName);
        _logger.LogInformation("TestAuth/Public - Claims count: {Count}", claims.Count);
        
        foreach (var claim in claims)
        {
            _logger.LogInformation("  Claim: {Claim}", claim);
        }

        ViewBag.IsAuthenticated = isAuth;
        ViewBag.UserName = userName;
        ViewBag.Claims = claims;
        
        return Content($@"
<!DOCTYPE html>
<html>
<head><title>Auth Test</title></head>
<body>
    <h1>Authentication Test - Public Page</h1>
    <p><strong>IsAuthenticated:</strong> {isAuth}</p>
    <p><strong>UserName:</strong> {userName}</p>
    <h2>Claims:</h2>
    <ul>
        {string.Join("", claims.Select(c => $"<li>{c}</li>"))}
    </ul>
    <hr>
    <a href='/TestAuth/Protected'>Test Protected Page</a> | 
    <a href='/Profile/Index'>Go to Profile</a> |
    <a href='/Auth/Login'>Login</a>
</body>
</html>
        ", "text/html");
    }

    // Protected endpoint - requires auth
    [Authorize]
    public IActionResult Protected()
    {
        var isAuth = User.Identity?.IsAuthenticated ?? false;
        var userName = User.Identity?.Name ?? "Not authenticated";
        var tokenClaim = User.FindFirst("AccessToken");
        var hasToken = !string.IsNullOrEmpty(tokenClaim?.Value);
        
        _logger.LogInformation("TestAuth/Protected - IsAuthenticated: {IsAuth}", isAuth);
        _logger.LogInformation("TestAuth/Protected - UserName: {UserName}", userName);
        _logger.LogInformation("TestAuth/Protected - Has AccessToken claim: {HasToken}", hasToken);
        
        if (hasToken)
        {
            _logger.LogInformation("TestAuth/Protected - Token (first 20 chars): {Token}", 
                tokenClaim!.Value.Substring(0, Math.Min(20, tokenClaim.Value.Length)));
        }

        return Content($@"
<!DOCTYPE html>
<html>
<head><title>Auth Test - Protected</title></head>
<body>
    <h1>Authentication Test - Protected Page</h1>
    <p style='color: green;'><strong>✓ You are authenticated!</strong></p>
    <p><strong>UserName:</strong> {userName}</p>
    <p><strong>Has AccessToken:</strong> {hasToken}</p>
    <hr>
    <a href='/TestAuth/Public'>Back to Public Page</a> | 
    <a href='/Profile/Index'>Go to Profile</a>
</body>
</html>
        ", "text/html");
    }
}
