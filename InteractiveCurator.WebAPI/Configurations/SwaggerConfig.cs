using Microsoft.OpenApi.Models;

namespace InteractiveCurator.WebAPI.Configurations
{
    public static class SwaggerConfig
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Steam App Data Collector API",
                    Version = "v1",
                    Description = "API for collecting and storing Steam app data in a Neo4j database."
                });
            });
        }
    }
}
