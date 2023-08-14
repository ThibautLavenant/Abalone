namespace Abalone
{
    public class Game
    {
        public List<Move> Moves { get; } = new List<Move>();
        public Dictionary<EPlayer, int> LostBalls { get; } = new Dictionary<EPlayer, int>();
        public Board Board { get; }
        public EPlayer CurrentPlayer { get; private set; }
        public EPlayer? Winner { get; private set; }

        public Game()
        {
            Board = new Board();
            LostBalls.Add(EPlayer.WHITE, 0);
            LostBalls.Add(EPlayer.BLACK, 0);
            CurrentPlayer = EPlayer.BLACK;
            Winner = null;
        }

        public void ExecuteMove(Move move)
        {
            //Check if game is won
            if (Winner != null)
            {
                return;
            }

            //Check if current player is origin
            if (CurrentPlayer != Board.BoardSpace[move.StartX, move.StartY])
            {
                return;
            }

            //Check if move is valid
            if (!Board.IsMoveValid(move))
            {
                return;
            }

            //Execute move on board and increment lostbals if needed
            var opponent = CurrentPlayer == EPlayer.WHITE ? EPlayer.BLACK : EPlayer.WHITE;
            var lostBall = Board.ExecuteMove(move);
            if (lostBall)
            {
                LostBalls[opponent]++;
            }

            //Store the move in the move list
            Moves.Add(move);

            if (LostBalls[opponent] == 6)
            {
                Winner = CurrentPlayer;
            }

            //Switch player
            CurrentPlayer = opponent;
        }

        public bool IsMoveValid(Move move)
        {
            //Check if game is won
            if (Winner != null)
            {
                return false;
            }

            //Check if current player is origin
            if (CurrentPlayer != Board.BoardSpace[move.StartX, move.StartY])
            {
                return false;
            }

            //Check if move is valid
            if (!Board.IsMoveValid(move))
            {
                return false;
            }

            return true;
        }
    }
}