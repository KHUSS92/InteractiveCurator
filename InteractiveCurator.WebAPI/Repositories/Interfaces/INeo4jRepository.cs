using InteractiveCurator.WebAPI.DTOs;
using InteractiveCurator.WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteractiveCurator.WebAPI.Repositories
{
    public interface INeo4jRepository
    {
        Task<List<Game>> GetRecommendedGamesAsync(List<Game> games);
    }
}
