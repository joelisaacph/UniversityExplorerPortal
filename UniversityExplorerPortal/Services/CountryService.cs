using System.Net.Http.Json;
using UniversityExplorerPortal.Models;

namespace UniversityExplorerPortal.Services;

// Talks to the Countries API: https://countries.dev/countries
public class CountryService
{
    private readonly HttpClient _http;

    public CountryService(HttpClient http)
    {
        _http = http;
    }

    // GET all countries (sorted A-Z). Throws an exception if the API call fails.
    public async Task<List<Country>> GetCountriesAsync()
    {
        var countries = await _http.GetFromJsonAsync<List<Country>>("countries") ?? new List<Country>();
        return countries.OrderBy(c => c.Name).ToList();
    }
}
