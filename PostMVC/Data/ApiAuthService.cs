using System.Text;
using System.Text.Json;
using PostMVC.Data.Service;
using PostMVC.Models;

namespace PostMVC.Data;

public class ApiAuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public ApiAuthService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["ApiSettings:BaseUrl"] ?? "http://api:5000";
    }

    public async Task<bool> Register(RegisterViewModel model)
    {
        var json = JsonSerializer.Serialize(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/Auth/register", content);
        return response.IsSuccessStatusCode;
    }

    public async Task<string?> Login(LoginViewModel model)
    {
        var json = JsonSerializer.Serialize(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/Auth/login", content);
        
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return tokenResponse?.Token;
    }

    private class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}
