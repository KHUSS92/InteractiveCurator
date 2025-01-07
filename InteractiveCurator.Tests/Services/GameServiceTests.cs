using Microsoft.Extensions.Logging;
using NUnit.Framework;
using NSubstitute;
using InteractiveCurator.WebAPI.Services;
using InteractiveCurator.WebAPI.Repositories;
using InteractiveCurator.WebAPI.DTOs;
using InteractiveCurator.WebAPI.Models;
using InteractiveCurator.WebAPI.Services.Interfaces;
using NSubstitute.ExceptionExtensions;

namespace InteractiveCurator.Tests.Services
{
    [TestFixture]
    public class GameServiceTests
    {
        private IGameService _gameService;
        private INeo4jRepository _neo4jRepository;
        private ISteamApiService _steamApiService;
        private ILogger<GameService> _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger<GameService>>();
            _neo4jRepository = Substitute.For<INeo4jRepository>();
            _steamApiService = Substitute.For<ISteamApiService>();
            _gameService = new GameService(_logger, _neo4jRepository, _steamApiService);
        }

        [Test]
        public async Task CurateGamesAsync_ValidInput_ReturnsRecommendations()
        {
            var inputGames = new List<string> { "Game1", "Game2" };

            var fetchedGameDetails = new List<GameDto>
            {
            new GameDto { Name = "Game1", SteamId = "1" },
            new GameDto { Name = "Game2", SteamId = "2" }
            };

            var recommendedGames = new List<Game>
            {
            new Game { Name = "Recommended1", SteamId = "101", Genre = "Action" },
            new Game { Name = "Recommended2", SteamId = "102", Genre = "Adventure" }
            };

            _steamApiService.GetGameDetailsAsync(Arg.Any<string>())
                .Returns(callInfo =>
                {
                    var gameName = (string)callInfo[0];
                    return Task.FromResult(fetchedGameDetails.FirstOrDefault(g => g.Name == gameName));
                });

            _neo4jRepository.GetRecommendedGamesAsync(Arg.Any<List<Game>>())
                .Returns(recommendedGames);

            var result = await _gameService.CurateGamesAsync(inputGames);

            Assert.That(result, Is.Not.Null, "The result should not be null.");
            Assert.That(result.Count, Is.EqualTo(2), "The result should contain 2 recommendations.");
            Assert.That(result[0].Name, Is.EqualTo("Recommended1"), "The first recommended game should be 'Recommended1'.");
            Assert.That(result[1].SteamId, Is.EqualTo("102"), "The second recommended game should have Steam ID '102'.");

            _logger.Received(1).LogInformation(
                "Starting game curation for input: {@Games}",
                Arg.Any<object>()
            );

            _logger.Received(1).LogInformation(
                "Generated recommendations: {@Recommendations}",
                Arg.Any<object>()
            );
        }

        [Test]
        public async Task CurateGamesAsync_NoInputGames_ReturnsEmptyList()
        {
            var inputGames = new List<string>();

            var result = await _gameService.CurateGamesAsync(inputGames);

            Assert.That(result, Is.Empty, "The result should be an empty list.");
            _logger.Received(1).LogInformation("Starting game curation for input: {@Games}", inputGames);
            _logger.Received(1).LogWarning("No games provided for curation.");
        }


        [Test]
        public void CurateGamesAsync_WhenExceptionThrown_LogsError()
        {
            var inputGames = new List<string> { "Game1" };

            _steamApiService.GetGameDetailsAsync(Arg.Any<string>())
                .Throws(new System.Exception("Test exception"));

            Assert.ThrowsAsync<System.Exception>(() => _gameService.CurateGamesAsync(inputGames));

            _logger.Received(1).LogError(Arg.Any<Exception>(), "Error occurred while curating games");
        }
    }
}
