using GamesApi.Interfaces.Services;
using GamesApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace GamesApi.Controllers
{
    // GamesController.cs
    [ApiController]
    [Route("api/games")]
    public class GamesController : ControllerBase
    {
        private readonly IGameParserService _gameParser;
        private readonly ILogReaderService _logReader;

        public GamesController(IGameParserService gameParser, ILogReaderService logReader)
        {
            _gameParser = gameParser;
            _logReader = logReader;
        }

        [HttpGet]
        public IActionResult GetAllGames()
        {
            var logContent = _logReader.ReadLogFile();
            var games = _gameParser.ParseGames(logContent);

            var result = games.Select(g => new GameResult
            {
                Name = g.Name,
                TotalKills = g.TotalKills,
                Players = g.Players,
                Kills = g.Kills
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetGameById(int id)
        {
            var logContent = _logReader.ReadLogFile();
            var games = _gameParser.ParseGames(logContent);

            var game = games.FirstOrDefault(g => g.Id == id);
            if (game == null) return NotFound();

            var result = new GameResult
            {
                Name = game.Name,
                TotalKills = game.TotalKills,
                Players = game.Players,
                Kills = game.Kills
            };

            return Ok(result);
        }
    }
}
