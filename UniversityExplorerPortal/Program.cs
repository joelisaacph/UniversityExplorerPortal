using UniversityExplorerPortal.Components;
using UniversityExplorerPortal.Services;

var builder = WebApplication.CreateBuilder(args);

// Blazor Web App with Interactive Server rendering
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// One HttpClient for each API. Each service class receives its own HttpClient.
builder.Services.AddHttpClient<CountryService>(client =>
    ConfigureClient(client, "https://countries.dev/"));

builder.Services.AddHttpClient<UniversityService>(client =>
    ConfigureClient(client, "http://universities.hipolabs.com/"));

builder.Services.AddHttpClient<StudentApplicationService>(client =>
    ConfigureClient(client, "https://edutots.net/"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

// Shared settings for every API client.
// Some web servers refuse requests that have no User-Agent header, so we always send one.
static void ConfigureClient(HttpClient client, string baseUrl)
{
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.UserAgent.ParseAdd("UniversityExplorerPortal/1.0");
    client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
}
