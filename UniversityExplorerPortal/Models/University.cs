using System.Text.Json.Serialization;

namespace UniversityExplorerPortal.Models;

// Matches the JSON returned by http://universities.hipolabs.com/search?country=...
// Some JSON names contain "-" or "_", so we map them with [JsonPropertyName].
public class University
{
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("state-province")]
    public string? StateProvince { get; set; }

    [JsonPropertyName("alpha_two_code")]
    public string? AlphaTwoCode { get; set; }

    public List<string> Domains { get; set; } = new();

    [JsonPropertyName("web_pages")]
    public List<string> WebPages { get; set; } = new();

    // Handy shortcut: the first website, or a friendly message if there is none.
    [JsonIgnore]
    public string Website => WebPages.FirstOrDefault() ?? "Not available";
}
