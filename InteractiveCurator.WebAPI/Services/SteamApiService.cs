using System.Text.Json;
using InteractiveCurator.WebAPI.Configurations;
using Microsoft.Extensions.Options;
using InteractiveCurator.WebAPI.Models;

namespace InteractiveCurator.WebAPI.Services
{
    public class SteamApiService : ISteamApiService
    {
        private readonly HttpClient _httpClient;
        private readonly SteamApiSettings _settings;

        public SteamApiService(HttpClient httpClient, IOptions<SteamApiSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<List<Game>> GetTopSellingGamesAsync(int limit = 1000)
        {
            var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/app/featured?api_key={_settings.ApiKey}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var games = JsonSerializer.Deserialize<List<Game>>(content);

            return games.Take(limit).ToList();
        
        }

        public async Task<Game> GetGameDetailsAsync(string appId)
        {
            var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/app/details/{appId}?api_key={_settings.ApiKey}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var game = JsonSerializer.Deserialize<Game>(content);

            return game;
        }

    }
}
