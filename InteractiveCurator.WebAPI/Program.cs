using InteractiveCurator.WebAPI.Configurations;
using InteractiveCurator.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add project-specific services and configurations
builder.Services.AddProjectServices(builder.Configuration);

// Add controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Steam App Data Collector API v1");
        c.RoutePrefix = string.Empty; // Makes Swagger available at the root
    });
}

app.UseAuthorization();
app.MapControllers();
app.Run();
