namespace GamesApi.Data.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name => $"game_{Id}";
        public int TotalKills { get; set; }
        public List<string> Players { get; set; } = new List<string>();
        public Dictionary<string, int> Kills { get; set; } = new Dictionary<string, int>();
        public List<string> KillHistory { get; set; } = new List<string>();
    }
}
