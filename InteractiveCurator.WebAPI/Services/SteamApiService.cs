using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using InteractiveCurator.WebAPI.DTOs;

namespace InteractiveCurator.WebAPI.Services
{
    public class SteamApiService : ISteamApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public SteamApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["SteamAPI:ApiKey"];
        }

        public async Task<GameDto> GetGameDetailsAsync(string gameId)
        {
            var url = $"https://api.steampowered.com/ISteamApps/GetAppDetails/v2?key={_apiKey}&appids={gameId}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            var gameData = JsonSerializer.Deserialize<GameDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return gameData;
        }
    }
}
