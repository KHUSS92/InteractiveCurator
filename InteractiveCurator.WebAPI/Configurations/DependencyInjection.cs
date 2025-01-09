using InteractiveCurator.WebAPI.Repositories;
using InteractiveCurator.WebAPI.Services;
using InteractiveCurator.WebAPI.Services.Interfaces;
using Neo4j.Driver;

namespace InteractiveCurator.WebAPI.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Neo4j configuration
            var neo4jConfig = configuration.GetSection("Neo4j").Get<Neo4jConfiguration>();
            var driver = GraphDatabase.Driver(neo4jConfig.Uri, AuthTokens.Basic(neo4jConfig.Username, neo4jConfig.Password));
            services.AddSingleton<IDriver>(driver);

            // Register services
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ISteamApiService, SteamApiService>();
            services.AddScoped<INeo4jRepository, Neo4jRepository>();

            // Register HttpClient for SteamApiService
            services.AddHttpClient<ISteamApiService, SteamApiService>();

            return services;
        }
    }
}
