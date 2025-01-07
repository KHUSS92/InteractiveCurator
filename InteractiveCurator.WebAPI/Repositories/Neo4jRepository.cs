using Neo4j.Driver;
using InteractiveCurator.WebAPI.DTOs;
using InteractiveCurator.WebAPI.Models;

namespace InteractiveCurator.WebAPI.Repositories
{
    public class Neo4jRepository : INeo4jRepository
    {
        private readonly IDriver _driver;

        public Neo4jRepository(IDriver driver)
        {
            _driver = driver;
        }

        public async Task<List<Game>> GetRecommendedGamesAsync(List<Game> games)
        {
            var recommendations = new List<Game>();

            using var session = _driver.AsyncSession();
            foreach (var game in games)
            {
                var query = @"
            MATCH (g:Game {name: $name})-[:ALSO_PLAYED]->(recommended:Game)
            RETURN recommended.name AS Name, recommended.steamId AS SteamId, recommended.genre AS Genre";

                var result = await session.RunAsync(query, new { name = game.Name });

                recommendations.AddRange(await result.ToListAsync(record => new Game
                {
                    Name = record["Name"].As<string>(),
                    SteamId = record["SteamId"].As<string>(),
                    Genre = record["Genre"].As<string>()
                }));
            }

            return recommendations;
        }
    }
}
