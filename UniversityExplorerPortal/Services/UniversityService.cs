using System.Net.Http.Json;
using UniversityExplorerPortal.Models;

namespace UniversityExplorerPortal.Services;

// Talks to the Universities API: http://universities.hipolabs.com/search?country={country}
public class UniversityService
{
    private readonly HttpClient _http;

    // The two APIs do not always spell a country the same way
    // (e.g. Countries API: "United States of America", Universities API: "United States").
    // This small list fixes the most common differences.
    private static readonly Dictionary<string, string> CountryNameFixes = new(StringComparer.OrdinalIgnoreCase)
    {
        { "United Kingdom of Great Britain and Northern Ireland", "United Kingdom" },
        { "United States of America", "United States" },
        { "Korea (Republic of)", "South Korea" },
        { "Viet Nam", "Vietnam" },
        { "Iran (Islamic Republic of)", "Iran" },
        { "Venezuela (Bolivarian Republic of)", "Venezuela" },
        { "Bolivia (Plurinational State of)", "Bolivia" },
        { "Tanzania, United Republic of", "Tanzania" },
        { "Syrian Arab Republic", "Syria" },
        { "Lao People's Democratic Republic", "Laos" },
    };

    public UniversityService(HttpClient http)
    {
        _http = http;
    }

    // GET all universities for one country. Throws an exception if the API call fails.
    public async Task<List<University>> GetUniversitiesAsync(string countryName)
    {
        // 1) Try the country name (using our small spelling fix list if needed)
        var nameToSearch = CountryNameFixes.TryGetValue(countryName, out var fixedName) ? fixedName : countryName;
        var universities = await SearchAsync(nameToSearch);

        // 2) Nothing found and the name has brackets, e.g. "Congo (Democratic Republic of the)"?
        //    Try again with just the part before the bracket.
        if (universities.Count == 0 && countryName.Contains('('))
        {
            var shortName = countryName.Substring(0, countryName.IndexOf('(')).Trim();
            universities = await SearchAsync(shortName);
        }

        return universities.OrderBy(u => u.Name).ToList();
    }

    private async Task<List<University>> SearchAsync(string country)
    {
        var url = $"search?country={Uri.EscapeDataString(country)}";
        return await _http.GetFromJsonAsync<List<University>>(url) ?? new List<University>();
    }
}
