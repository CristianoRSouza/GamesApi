using GamesApi.Services;

namespace GamesTests.TestsServices
{
    public class GameParserServiceTests
    {
        [Fact]
        public void ParseGames_HandlesMultipleGames()
        {
            string log = @"InitGame:
                      Kill: 1 killed 2 by MOD_RAILGUN
                      InitGame:
                      Kill: 2 killed 1 by MOD_SHOTGUN";

            var parser = new GameParserService();
            var games = parser.ParseGames(log);

            Assert.Equal(2, games.Count);
            Assert.Equal(1, games[0].TotalKills);
            Assert.Equal(1, games[1].TotalKills);
        }
    }

}
