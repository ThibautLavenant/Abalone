namespace Abalone
{
    public class Game
    {
        public List<Move> Moves { get; } = new List<Move>();
        public Dictionary<Player, int> LostBalls { get; } = new Dictionary<Player, int>();
        public Board Board { get; set; }

        public Game()
        {
            Board = new Board();
            LostBalls.Add(Player.WHITE, 0);
            LostBalls.Add(Player.BLACK, 0);
        }
    }
}