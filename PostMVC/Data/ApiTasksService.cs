using System.Text.Json;
using PostMVC.Data.Service;
using PostMVC.Models;

namespace PostMVC.Data;

public class ApiTasksService : ITasksService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public ApiTasksService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["ApiSettings:BaseUrl"] ?? "http://api:5000";
    }

    public async Task<IEnumerable<Tasks>> GetAll()
    {
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
        var json = JsonSerializer.Serialize(task);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/Tasks", content);
        response.EnsureSuccessStatusCode();
    }
}
