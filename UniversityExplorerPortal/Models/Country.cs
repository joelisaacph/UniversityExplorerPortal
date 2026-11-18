namespace UniversityExplorerPortal.Models;

// Matches the JSON returned by https://countries.dev/countries
// (we only keep the fields we actually need).
public class Country
{
    public string Name { get; set; } = string.Empty;
    public string? Capital { get; set; }
    public string? Region { get; set; }
    public long Population { get; set; }

    // The flag as an emoji, e.g. 🇮🇪
    public string? Flag { get; set; }

    // The flag as an image (png / svg link)
    public CountryFlags? Flags { get; set; }
}

public class CountryFlags
{
    public string? Png { get; set; }
    public string? Svg { get; set; }
}
