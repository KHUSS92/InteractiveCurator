using Neo4j.Driver;
using InteractiveCurator.WebAPI.Models;

namespace InteractiveCurator.WebAPI.Helpers
{
    public static class Neo4jHelper
    {
        public static async Task<T?> FetchSingleAsync<T>(IAsyncSession session, string query, object parameters, Func<IRecord, T> mapper)
        {
            var result = await session.RunAsync(query, parameters);

            if (await result.FetchAsync())
            {
                return mapper(result.Current);
            }

            return default;
        }

        public static async Task<List<T>> FetchMultipleAsync<T>(IAsyncSession session, string query, object parameters, Func<IRecord, T> mapper)
        {
            var result = await session.RunAsync(query, parameters);

            return (await result.ToListAsync())
                .Select(mapper)
                .ToList();
        }

        public static Game MapNodeToGame(INode node)
        {
            return new Game
            {
                AppId = node["appId"].As<string>(),
                Name = node["name"].As<string>(),
                Description = node["description"].As<string>(),
                Developer = node["developer"].As<string>(),
                Publisher = node["publisher"].As<string>(),
                ReleaseDate = node["releaseDate"].As<string>(),
                Price = node["price"].As<decimal>(),
                Genres = node.Properties.ContainsKey("genres") ? node["genres"].As<List<string>>() : new List<string>(),
                Categories = node.Properties.ContainsKey("categories") ? node["categories"].As<List<string>>() : new List<string>()
            };
        }
    }
}
