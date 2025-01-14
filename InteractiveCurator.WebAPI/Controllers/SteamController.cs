using InteractiveCurator.WebAPI.Models;
using InteractiveCurator.WebAPI.Repositories;
using InteractiveCurator.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveCurator.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SteamController : ControllerBase
    {
        private readonly ISteamService _steamService;
        private readonly INeo4jRepository _neo4jRepository;

        public SteamController(ISteamService steamService, INeo4jRepository neo4jRepository)
        {
            _steamService = steamService;
            _neo4jRepository = neo4jRepository;
        }

        [HttpPost("fetch-and-store")]
        public async Task<IActionResult> FetchAndStoreData()
        {
            var appList = await _steamService.GetAppListAsync();

            foreach (var app in appList)
            {
                if (await _neo4jRepository.AppExistsAsync(app.AppId))
                    continue;

                var appDetails = await _steamService.GetAppDetailsAsync(app.AppId);
                if (appDetails == null)
                    continue;

                await _neo4jRepository.AddAppAsync(new Neo4jApp
                {
                    AppId = appDetails.AppId,
                    Name = appDetails.Name,
                    ShortDescription = appDetails.ShortDescription,
                    Genres = appDetails.Genres
                });
            }

            return Ok(new { message = "Data fetched and stored successfully!" });
        }
    }
}