using System.Text.Json;
using InteractiveCurator.WebAPI.Configurations;
using Microsoft.Extensions.Options;
using InteractiveCurator.WebAPI.Models;

namespace InteractiveCurator.WebAPI.Services
{

    public class SteamService : ISteamService
    {
        private readonly HttpClient _httpClient;
        private readonly SteamApiConfig _config;

        public SteamService(HttpClient httpClient, IOptions<SteamApiConfig> config)
        {
            _httpClient = httpClient;
            _config = config.Value;
        }

        public async Task<List<AppList>?> GetAppListAsync()
        {
            var response = await _httpClient.GetStringAsync(_config.GetAppListUrl);

            var jsonDocument = JsonDocument.Parse(response); 
            var rootElement = jsonDocument.RootElement;

            if (rootElement.TryGetProperty("applist", out JsonElement appListElement) &&
                appListElement.TryGetProperty("apps", out JsonElement appsElement))
            {
                return appsElement.EnumerateArray()
                    .Take(1000) 
                    .Select(app => new AppList
                    {
                        AppId = app.GetProperty("appid").GetInt32(),
                        Name = app.GetProperty("name").GetString()
                    })
                    .ToList();
            }
            else
            {
                throw new Exception("Invalid JSON structure: 'applist' or 'apps' is missing.");
            }

        }

        public async Task<AppDetail> GetAppDetailsAsync(int appId)
        {
            var url = _config.GetAppDetailsUrl.Replace("{appId}", appId.ToString());
            var response = await _httpClient.GetStringAsync(url);


            using var jsonDocument = JsonDocument.Parse(response);
            var rootElement = jsonDocument.RootElement;

            if (!rootElement.TryGetProperty(appId.ToString(), out JsonElement appElement))
            {
                return null;
            }

            if (!appElement.TryGetProperty("data", out JsonElement appData))
            {
                return null; 
            }

            if (appData.TryGetProperty("type", out JsonElement typeElement) &&
                  typeElement.GetString() != "game")
            {
                return null; 
            }

            var genres = new List<string>();
            if (appData.TryGetProperty("genres", out JsonElement genresElement) &&
                genresElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var genreElement in genresElement.EnumerateArray())
                {
                    if (genreElement.TryGetProperty("description", out JsonElement descriptionElement))
                    {
                        genres.Add(descriptionElement.GetString() ?? "Unknown");
                    }
                }
            }

            return new AppDetail
            {
                AppId = appId,
                Name = appData.GetProperty("name").GetString() ?? "Unknown",
                ShortDescription = appData.GetProperty("short_description").GetString() ?? "No description",
                Genres = genres
            };
        }
    }
}

