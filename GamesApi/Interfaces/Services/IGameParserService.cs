using GamesApi.Data.Models;

namespace GamesApi.Interfaces.Services
{
    public interface IGameParserService
    {
        List<Game> ParseGames(string logContent);
    }
}
