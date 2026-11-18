using System.Net.Http.Json;
using UniversityExplorerPortal.Models;

namespace UniversityExplorerPortal.Services;

// Talks to the Student Application API: https://edutots.net/api/studentapplication
//   GET  api/studentapplication        -> all applications
//   POST api/studentapplication        -> create an application
//   PUT  api/studentapplication/{id}   -> update an application
public class StudentApplicationService
{
    private const string ApiPath = "api/studentapplication";
    private readonly HttpClient _http;

    public StudentApplicationService(HttpClient http)
    {
        _http = http;
    }

    // GET: all applications (lowest Id first)
    public async Task<List<StudentApplication>> GetApplicationsAsync()
    {
        var applications = await _http.GetFromJsonAsync<List<StudentApplication>>(ApiPath)
                           ?? new List<StudentApplication>();
        return applications.OrderBy(a => a.Id).ToList();
    }

    // GET: one application. The coursework only lists "GET all", so we load the list and pick the one we need.
    public async Task<StudentApplication?> GetApplicationAsync(int id)
    {
        var applications = await GetApplicationsAsync();
        return applications.FirstOrDefault(a => a.Id == id);
    }

    // POST: create a new application
    public async Task CreateApplicationAsync(StudentApplication application)
    {
        var response = await _http.PostAsJsonAsync(ApiPath, application);
        await ThrowIfNotSuccessAsync(response);
    }

    // PUT: update an existing application
    public async Task UpdateApplicationAsync(StudentApplication application)
    {
        var response = await _http.PutAsJsonAsync($"{ApiPath}/{application.Id}", application);
        await ThrowIfNotSuccessAsync(response);
    }

    // If the API says "no", throw an exception that includes the API's own message.
    // The pages catch it and show a friendly error to the user.
    private static async Task ThrowIfNotSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync();
        if (body.Length > 300)
        {
            body = body.Substring(0, 300) + "...";
        }

        throw new HttpRequestException($"The API returned {(int)response.StatusCode} {response.ReasonPhrase}. {body}");
    }
}
