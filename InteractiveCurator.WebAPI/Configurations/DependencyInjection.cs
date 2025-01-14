using InteractiveCurator.WebAPI.Repositories;
using InteractiveCurator.WebAPI.Configurations;
using InteractiveCurator.WebAPI.Services;
using Neo4j.Driver;

namespace InteractiveCurator.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static void AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind configuration classes
            services.Configure<SteamApiConfig>(configuration.GetSection("SteamApi"));
            services.Configure<Neo4jConfiguration>(configuration.GetSection("Neo4j"));

            // Add services
            services.AddScoped<ISteamService, SteamService>();

            // Register HttpClient for SteamService
            services.AddHttpClient<ISteamService, SteamService>();

            // Configure Neo4j driver
            var neo4jConfig = configuration.GetSection("Neo4j").Get<Neo4jConfiguration>();
            var driver = GraphDatabase.Driver(neo4jConfig.Uri, AuthTokens.Basic(neo4jConfig.Username, neo4jConfig.Password));
            services.AddSingleton<IDriver>(driver);

            // Add repositories
            services.AddScoped<INeo4jRepository, Neo4jRepository>();

            // Add Swagger
            services.AddSwaggerGen();
        }
    }
}
