using FluentAssertions;
using GamesApi.Controllers;
using GamesApi.Data.Models;
using GamesApi.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GamesTests.TestsControllers
{
    public class GamesTestsController
    {
        private readonly Mock<IGameParserService> _mockParserService;
        private readonly Mock<ILogReaderService> _mockLogReaderService;
        private readonly GamesController _controller;

        public GamesTestsController()
        {
            _mockParserService = new Mock<IGameParserService>();
            _mockLogReaderService = new Mock<ILogReaderService>();
            _controller = new GamesController(_mockParserService.Object, _mockLogReaderService.Object);
        }

        [Fact]
        public void GetAllGames_ShouldReturnOkResult_WithListOfGames()
        {
            var mockGames = new List<Game>
            {
                new Game { Id = 1,  TotalKills = 10, Players = new List<string> { "Player1" }, Kills = new Dictionary<string, int> { ["Player1"] = 5 } },
                new Game { Id = 2, TotalKills = 15, Players = new List<string> { "Player2" }, Kills = new Dictionary<string, int> { ["Player2"] = 8 } }
            };

            _mockLogReaderService.Setup(x => x.ReadLogFile()).Returns("mock log content");
            _mockParserService.Setup(x => x.ParseGames(It.IsAny<string>())).Returns(mockGames);
            var result = _controller.GetAllGames();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeAssignableTo<IEnumerable<GameResult>>();
            var games = okResult.Value as IEnumerable<GameResult>;
            games.Should().HaveCount(2);
        }

        [Fact]
        public void GetGameById_ShouldReturnOkResult_WhenGameExists()
        {
            var mockGame = new Game { Id = 1, TotalKills = 10, Players = new List<string> { "Player1" }, Kills = new Dictionary<string, int> { ["Player1"] = 5 } };
            var mockGames = new List<Game> { mockGame };

            _mockLogReaderService.Setup(x => x.ReadLogFile()).Returns("mock log content");
            _mockParserService.Setup(x => x.ParseGames(It.IsAny<string>())).Returns(mockGames);

            var result = _controller.GetGameById(1);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeOfType<GameResult>();
            var gameResult = okResult.Value as GameResult;
            gameResult.Name.Should().Be("game_1");
        }

        [Fact]
        public void GetGameById_ShouldReturnNotFound_WhenGameDoesNotExist()
        {
            var mockGames = new List<Game>
            {
                new Game { Id = 1 }
            };

            _mockLogReaderService.Setup(x => x.ReadLogFile()).Returns("mock log content");
            _mockParserService.Setup(x => x.ParseGames(It.IsAny<string>())).Returns(mockGames);

            var result = _controller.GetGameById(99);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetAllGames_ShouldReturnEmptyList_WhenNoGamesExist()
        {
            _mockLogReaderService.Setup(x => x.ReadLogFile()).Returns("mock log content");
            _mockParserService.Setup(x => x.ParseGames(It.IsAny<string>())).Returns(new List<Game>());

            var result = _controller.GetAllGames();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeAssignableTo<IEnumerable<GameResult>>();
            var games = okResult.Value as IEnumerable<GameResult>;
            games.Should().BeEmpty();
        }

        [Fact]
        public void GetAllGames_ShouldCallServicesOnce()
        {
            _mockLogReaderService.Setup(x => x.ReadLogFile()).Returns("mock log content");
            _mockParserService.Setup(x => x.ParseGames(It.IsAny<string>())).Returns(new List<Game>());

            _controller.GetAllGames();

            _mockLogReaderService.Verify(x => x.ReadLogFile(), Times.Once);
            _mockParserService.Verify(x => x.ParseGames(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GetGameById_ShouldCallServicesOnce()
        {
            var mockGame = new Game { Id = 1 };
            var mockGames = new List<Game> { mockGame };

            _mockLogReaderService.Setup(x => x.ReadLogFile()).Returns("mock log content");
            _mockParserService.Setup(x => x.ParseGames(It.IsAny<string>())).Returns(mockGames);

            _controller.GetGameById(1);

            _mockLogReaderService.Verify(x => x.ReadLogFile(), Times.Once);
            _mockParserService.Verify(x => x.ParseGames(It.IsAny<string>()), Times.Once);
        }
    }
}