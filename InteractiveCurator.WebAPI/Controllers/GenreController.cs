using InteractiveCurator.WebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/genres")]
public class GenreController : ControllerBase
{
    private readonly INeo4jRepository _repository;

    public GenreController(INeo4jRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetGenres()
    {
        var genres = await _repository.GetAllGenresAsync();
        return Ok(genres);
    }

    [HttpGet("{genre}")]
    public async Task<IActionResult> GetAppsByGenre(string genre)
    {
        var apps = await _repository.GetAppsByGenreAsync(genre);
        if (apps == null) return NotFound();
        return Ok(apps);
    }
}
