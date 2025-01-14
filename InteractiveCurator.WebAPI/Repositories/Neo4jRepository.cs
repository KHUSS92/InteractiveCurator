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
}