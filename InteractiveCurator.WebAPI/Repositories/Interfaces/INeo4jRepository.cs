using InteractiveCurator.WebAPI.Models;

namespace InteractiveCurator.WebAPI.Repositories
{
    public interface INeo4jRepository
    {
        Task<bool> AppExistsAsync(int appId);
        Task AddAppAsync(Neo4jApp app);
    }
}
