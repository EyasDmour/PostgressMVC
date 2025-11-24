using System.Text.Json;
using PostMVC.Data.Service;
using PostMVC.Models;

namespace PostMVC.Data;

public class ApiProjectsService : IProjectsService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public ApiProjectsService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["ApiSettings:BaseUrl"] ?? "http://api:5000";
    }

    public async Task<IEnumerable<Projects>> GetAll()
    {
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
        var json = JsonSerializer.Serialize(project);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/Projects", content);
        response.EnsureSuccessStatusCode();
    }

}