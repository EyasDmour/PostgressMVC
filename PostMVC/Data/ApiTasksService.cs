using System.Net.Http.Headers;
using System.Text.Json;
using PostMVC.Data.Service;
using PostMVC.Models;

namespace PostMVC.Data;

public class ApiTasksService : ITasksService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiTasksService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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

    public async Task<IEnumerable<Tasks>> GetAll()
    {
        AddAuthorizationHeader();
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/Tasks");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var tasks = JsonSerializer.Deserialize<IEnumerable<Tasks>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return tasks ?? new List<Tasks>();
    }

    public async Task Add(Tasks task)
    {
        AddAuthorizationHeader();
        var json = JsonSerializer.Serialize(task);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/Tasks", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task Update(Tasks task)
    {
        AddAuthorizationHeader();
        var json = JsonSerializer.Serialize(task);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"{_baseUrl}/api/Tasks/{task.Id}", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(int id)
    {
        AddAuthorizationHeader();
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/Tasks/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<Tasks?> GetById(int id)
    {
        AddAuthorizationHeader();
        var allTasks = await GetAll();
        return allTasks.FirstOrDefault(t => t.Id == id);
    }
}
