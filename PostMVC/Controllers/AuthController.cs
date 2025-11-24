using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PostMVC.Data.Service;
using PostMVC.Models;

namespace PostMVC.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var token = await _authService.Login(model);
        if (token == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        var claims = ParseClaimsFromJwt(token);
        // Ensure we have at least the name
        if (!claims.Any(c => c.Type == ClaimTypes.Name))
        {
             claims.Add(new Claim(ClaimTypes.Name, model.Username));
        }
        claims.Add(new Claim("JWT", token));

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTime.UtcNow.AddHours(1) // Simplified expiry
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return RedirectToAction("Index", "Home");
    }

    private List<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        
        var claims = new List<Claim>();
        if (keyValuePairs == null) return claims;

        foreach (var kvp in keyValuePairs)
        {
            var value = kvp.Value.ToString() ?? "";
            // Map standard JWT claims to .NET ClaimTypes
            if (kvp.Key == "unique_name") 
            {
                claims.Add(new Claim(ClaimTypes.Name, value));
            }
            else if (kvp.Key == "role")
            {
                claims.Add(new Claim(ClaimTypes.Role, value));
            }
            else
            {
                claims.Add(new Claim(kvp.Key, value));
            }
        }
        return claims;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _authService.Register(model);
        if (result)
        {
            return RedirectToAction("Login");
        }

        ModelState.AddModelError(string.Empty, "Registration failed. Username might already be taken.");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
