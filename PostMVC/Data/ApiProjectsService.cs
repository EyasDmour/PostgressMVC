using System.Net.Http.Headers;
using System.Text.Json;
using PostMVC.Data.Service;
using PostMVC.Models;

namespace PostMVC.Data;

public class ApiProjectsService : IProjectsService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiProjectsService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["ApiSettings:BaseUrl"] ?? "http://api:5000";
        _httpContextAccessor = httpContextAccessor;
    }

    private void AddAuthorizationHeader()
    {
        var token = _httpContextAccessor.HttpContext?.User.FindFirst("JWT")?.Value;
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<IEnumerable<Projects>> GetAll()
    {
        AddAuthorizationHeader();
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/Projects");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var projects = JsonSerializer.Deserialize<IEnumerable<Projects>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return projects ?? new List<Projects>();
    }

    public async Task Add(Projects project)
    {
        AddAuthorizationHeader();
        var json = JsonSerializer.Serialize(project);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/Projects", content);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"API Error: {response.StatusCode} - {errorContent}");
        }
    }

    public async Task Delete(int id)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/Projects/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task AddMember(int projectId, string username)
    {
        AddAuthorizationHeader();
        var json = JsonSerializer.Serialize(username);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/Projects/{projectId}/members", content);
        response.EnsureSuccessStatusCode();
    }
}