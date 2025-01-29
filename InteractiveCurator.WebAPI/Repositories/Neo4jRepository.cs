using Neo4j.Driver;
using InteractiveCurator.WebAPI.Models;
using InteractiveCurator.WebAPI.Repositories;

public class Neo4jRepository : INeo4jRepository
{
    private readonly IDriver _driver;

    public Neo4jRepository(IDriver driver)
    {
        _driver = driver;
    }

    public async Task<bool> AppExistsAsync(int appId)
    {
        var query = "MATCH (app:App {AppId: $appId}) RETURN app LIMIT 1";
        using var session = _driver.AsyncSession();
        var result = await session.RunAsync(query, new { appId });
        return await result.FetchAsync();
    }

    public async Task AddAppAsync(Neo4jApp app)
    {
        var query = @"
                CREATE (app:App {
                    AppId: $appId,
                    Name: $name,
                    ShortDescription: $shortDescription,
                    Genres: $genres
                })";

        using var session = _driver.AsyncSession();
        await session.RunAsync(query, new
        {
            appId = app.AppId,
            name = app.Name,
            shortDescription = app.ShortDescription,
            genres = app.Genres
        });
    }

    public async Task<List<GenreWithApps>> GetAllGenresAsync()
    {
        var genresWithApps = new List<GenreWithApps>();

        var query = @"
            MATCH (g:Genre)<-[:HAS_GENRE]-(a:App)
            RETURN g.Name AS Genre, COLLECT({ appId: a.AppId, name: a.Name }) AS Apps";

        var session = _driver.AsyncSession();
        try
        {
            var result = await session.RunAsync(query);
            await result.ForEachAsync(record =>
            {
                genresWithApps.Add(new GenreWithApps
                {
                    Genre = record["Genre"].As<string>(),
                    Apps = record["Apps"].As<List<Dictionary<string, object>>>()
                });
            });
        }
        finally
        {
            await session.CloseAsync();
        }

        return genresWithApps;
    }

    public async Task<List<AppDTO>> GetAppsByGenreAsync(string genre)
    {
        var apps = new List<AppDTO>();

        var query = @"
            MATCH (a:App)-[:HAS_GENRE]->(g:Genre {Name: $genre})
            RETURN a.AppId AS appId, a.Name AS name";

        var session = _driver.AsyncSession();
        try
        {
            var result = await session.RunAsync(query, new { genre });
            await result.ForEachAsync(record =>
            {
                apps.Add(new AppDTO
                {
                    AppId = record["appId"].As<int>(),
                    Name = record["name"].As<string>()
                });
            });
        }
        finally
        {
            await session.CloseAsync();
        }

        return apps;
    }
}