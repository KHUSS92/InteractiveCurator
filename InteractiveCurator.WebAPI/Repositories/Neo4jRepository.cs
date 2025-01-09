using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;
using InteractiveCurator.WebAPI.Models;
using InteractiveCurator.WebAPI.Helpers;
using InteractiveCurator.WebAPI.Repositories;

public class Neo4jRepository : INeo4jRepository
{
    private readonly IAsyncSession _session;

    public Neo4jRepository(IAsyncSession session)
    {
        _session = session;
    }

    /// <summary>
    /// Adds a game to the Neo4j database and creates relationships for genres and categories.
    /// </summary>
    public async Task AddGameAsync(Game game)
    {
        var query = @"
            MERGE (g:Game {appId: $appId})
            SET g.name = $name,
                g.description = $description,
                g.releaseDate = $releaseDate,
                g.developer = $developer,
                g.publisher = $publisher,
                g.price = $price
            WITH g
            UNWIND $genres AS genre
            MERGE (gen:Genre {name: genre})
            MERGE (g)-[:BELONGS_TO]->(gen)
            WITH g
            UNWIND $categories AS category
            MERGE (cat:Category {name: category})
            MERGE (g)-[:HAS_CATEGORY]->(cat)";

        await _session.RunAsync(query, new
        {
            appId = game.AppId,
            name = game.Name,
            description = game.Description,
            releaseDate = game.ReleaseDate,
            developer = game.Developer,
            publisher = game.Publisher,
            price = game.Price,
            genres = game.Genres,
            categories = game.Categories
        });
    }

    /// <summary>
    /// Retrieves a game from the database by its App ID.
    /// </summary>
    public async Task<Game?> GetGameByAppIdAsync(string appId)
    {
        var query = @"
            MATCH (g:Game {appId: $appId})
            RETURN g";

        return await Neo4jHelper.FetchSingleAsync(_session, query, new { appId }, record =>
            Neo4jHelper.MapNodeToGame(record["g"].As<INode>()));
    }

    /// <summary>
    /// Retrieves recommendations for a given game based on genres and categories.
    /// </summary>
    public async Task<List<Game>> GetRecommendationsForGameAsync(Game game)
    {
        var query = @"
            MATCH (g:Game {appId: $appId})-[:BELONGS_TO|:HAS_CATEGORY]->(shared)<-[:BELONGS_TO|:HAS_CATEGORY]-(rec:Game)
            WHERE g <> rec
            RETURN DISTINCT rec";

        return await Neo4jHelper.FetchMultipleAsync(_session, query, new { appId = game.AppId }, record =>
            Neo4jHelper.MapNodeToGame(record["rec"].As<INode>()));
    }
}
