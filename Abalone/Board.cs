namespace Abalone
{
    public class Board
    {
        public Player?[,] BoardSpace { get; } = new Player?[9, 9];

        public Board()
        {
            InitBoard();
        }

        private void InitBoard()
        {
            BoardSpace[0, 0] = Player.BLACK;
            BoardSpace[0, 1] = Player.BLACK;
            BoardSpace[0, 2] = Player.BLACK;
            BoardSpace[0, 3] = Player.BLACK;
            BoardSpace[0, 4] = Player.BLACK;

            BoardSpace[1, 0] = Player.BLACK;
            BoardSpace[1, 1] = Player.BLACK;
            BoardSpace[1, 2] = Player.BLACK;
            BoardSpace[1, 3] = Player.BLACK;
            BoardSpace[1, 4] = Player.BLACK;
            BoardSpace[1, 5] = Player.BLACK;

            BoardSpace[2, 2] = Player.BLACK;
            BoardSpace[2, 3] = Player.BLACK;
            BoardSpace[2, 4] = Player.BLACK;


            BoardSpace[8, 4] = Player.WHITE;
            BoardSpace[8, 5] = Player.WHITE;
            BoardSpace[8, 6] = Player.WHITE;
            BoardSpace[8, 7] = Player.WHITE;
            BoardSpace[8, 8] = Player.WHITE;

            BoardSpace[7, 3] = Player.WHITE;
            BoardSpace[7, 4] = Player.WHITE;
            BoardSpace[7, 5] = Player.WHITE;
            BoardSpace[7, 6] = Player.WHITE;
            BoardSpace[7, 7] = Player.WHITE;
            BoardSpace[7, 8] = Player.WHITE;

            BoardSpace[6, 4] = Player.WHITE;
            BoardSpace[6, 5] = Player.WHITE;
            BoardSpace[6, 6] = Player.WHITE;
        }

        private bool IsPositionValid(int x, int y)
        {
            if (x < 0 || x > 8 || y < 0 || y > 8)
            {
                return false;
            }
            if (y > x + 4 || y < x - 4)
            {
                return false;
            }
            return true;
        }

        public void ExecuteMove(Move move)
        {
            //Position de lecture actuelle
            int currentX;
            int currentY;
            //Vecteur de déplacement
            int movementX;
            int movementY;
            //Position finale
            int finalX;
            int finalY;
            //Déplacement itératif
            int stepX;
            int stepY;

            movementX = move.EndX - move.StartX;
            movementY = move.EndY - move.StartY;
            currentX = move.RangeX + movementX;
            currentY = move.RangeY + movementY;
            stepX = move.StartX - move.RangeX;
            stepY = move.StartY - move.RangeY;
            stepX = stepX == 0 ? 0 : stepX / Math.Abs(stepX);
            stepY = stepY == 0 ? 0 : stepY / Math.Abs(stepY);
            finalX = move.EndX + stepX;
            finalY = move.EndY + stepY;

            do
            {
                BoardSpace[currentX, currentY] = BoardSpace[currentX - movementX, currentY - movementY];
                BoardSpace[currentX - movementX, currentY - movementY] = null;

                currentX += stepX;
                currentY += stepY;
            } while (currentX != finalX || currentY != finalY);
        }
    }
}