using GamesApi.Data.Models;
using GamesApi.Interfaces.Services;
using System.Reflection.Metadata;

namespace GamesApi.Services
{
    public class GameParserService : IGameParserService
    {
        public List<Game> ParseGames(string logContent)
        {
            var games = new List<Game>();
            Game currentGame = null;

            using (var reader = new StringReader(logContent))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("InitGame"))
                    {
                        currentGame = new Game { Id = games.Count + 1 };
                        games.Add(currentGame);
                    }
                    else if (line.Contains("Kill:"))
                    {
                        ProcessKillLine(line, currentGame);
                    }
                }
            }

            return games;
        }

        private void ProcessKillLine(string line, Game game)
        {
            game.TotalKills++;

            var killInfo = line.Split(new[] { "Kill: " }, StringSplitOptions.None)[1];
            var parts = killInfo.Split(new[] { " killed ", " by " }, StringSplitOptions.None);

            var killer = parts[0].Trim();
            var victim = parts[1].Trim();
            var mod = parts[2].Trim();

            if (killer != "<world>" && !game.Players.Contains(killer))
                game.Players.Add(killer);

            if (!game.Players.Contains(victim))
                game.Players.Add(victim);

            if (killer == "<world>")
            {
                if (game.Kills.ContainsKey(victim))
                    game.Kills[victim]--;
                else
                    game.Kills[victim] = -1;
            }
            else if (killer != victim) 
            {
                if (game.Kills.ContainsKey(killer))
                    game.Kills[killer]++;
                else
                    game.Kills[killer] = 1;
            }

            game.KillHistory.Add($"{killer} killed {victim} with {mod}");
        }
    }
}
