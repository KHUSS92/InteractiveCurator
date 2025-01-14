using InteractiveCurator.WebAPI.Models;

namespace InteractiveCurator.WebAPI.Services
{
    public interface ISteamService
    {
        Task<List<AppList>?> GetAppListAsync();
        Task<AppDetail> GetAppDetailsAsync(int appId);
    }
}
