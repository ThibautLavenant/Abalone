using System.Diagnostics.SymbolStore;

namespace Abalone
{
    public class Board
    {
        public EPlayer?[,] BoardSpace { get; } = new EPlayer?[9, 9];

        public Board()
        {
            InitBoard();
        }

        private void InitBoard()
        {
            BoardSpace[0, 0] = EPlayer.BLACK;
            BoardSpace[0, 1] = EPlayer.BLACK;
            BoardSpace[0, 2] = EPlayer.BLACK;
            BoardSpace[0, 3] = EPlayer.BLACK;
            BoardSpace[0, 4] = EPlayer.BLACK;

            BoardSpace[1, 0] = EPlayer.BLACK;
            BoardSpace[1, 1] = EPlayer.BLACK;
            BoardSpace[1, 2] = EPlayer.BLACK;
            BoardSpace[1, 3] = EPlayer.BLACK;
            BoardSpace[1, 4] = EPlayer.BLACK;
            BoardSpace[1, 5] = EPlayer.BLACK;

            BoardSpace[2, 2] = EPlayer.BLACK;
            BoardSpace[2, 3] = EPlayer.BLACK;
            BoardSpace[2, 4] = EPlayer.BLACK;


            BoardSpace[8, 4] = EPlayer.WHITE;
            BoardSpace[8, 5] = EPlayer.WHITE;
            BoardSpace[8, 6] = EPlayer.WHITE;
            BoardSpace[8, 7] = EPlayer.WHITE;
            BoardSpace[8, 8] = EPlayer.WHITE;

            BoardSpace[7, 3] = EPlayer.WHITE;
            BoardSpace[7, 4] = EPlayer.WHITE;
            BoardSpace[7, 5] = EPlayer.WHITE;
            BoardSpace[7, 6] = EPlayer.WHITE;
            BoardSpace[7, 7] = EPlayer.WHITE;
            BoardSpace[7, 8] = EPlayer.WHITE;

            BoardSpace[6, 4] = EPlayer.WHITE;
            BoardSpace[6, 5] = EPlayer.WHITE;
            BoardSpace[6, 6] = EPlayer.WHITE;
        }

        internal bool ExecuteMove(Move move)
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
            while(IsPositionValid(currentX + movementX, currentY + movementY) 
                && BoardSpace[currentX, currentY] != null)
            {
                currentX += movementX;
                currentY += movementY;
            }
            bool retVal = false;
            if (BoardSpace[currentX, currentY] != null)
            {
                retVal = true;
            }
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

            return retVal;
        }

        #region PRIVATE HELPERS

        internal bool IsMoveValid(Move move)
        {
            if (move == null) return false;

            //Check if movement start end is valid
            if (!IsPositionValid(move.StartX, move.StartY)
                || !IsPositionValid(move.EndX, move.EndY)
                || !IsPositionValid(move.RangeX, move.RangeY))
            {
                return false;
            }

            //Get player color
            var moveColor = BoardSpace[move.StartX, move.StartY];
            if (moveColor == null)
            {
                return false;
            }
            var opponentColor = moveColor == EPlayer.BLACK ? EPlayer.WHITE : EPlayer.BLACK;

            //Check if movement vector is valid
            var movementX = move.EndX - move.StartX;
            var movementY = move.EndY - move.StartY;
            if (!IsVectorValid(movementX, movementY))
            {
                return false;
            }

            //Check if range is valid
            var rangeVectorX = move.RangeX - move.StartX;
            var rangeVectorY = move.RangeY - move.StartY;
            var rangeIncrementVectorX = rangeVectorX == 0 ? 0 : rangeVectorX / Math.Abs(rangeVectorX);
            var rangeIncrementVectorY = rangeVectorY == 0 ? 0 : rangeVectorY / Math.Abs(rangeVectorY);
            var rangeVectorLength = Math.Max(Math.Abs(rangeVectorX), Math.Abs(rangeVectorY));
            if (!IsRangeValid(rangeVectorX, rangeVectorY))
            {
                return false;
            }

            //Check if there is only player color in range
            for (int i = 0; i < rangeVectorLength; i++)
            {
                if (BoardSpace[move.StartX + rangeIncrementVectorX * (i + 1), move.StartY + rangeIncrementVectorY * (i + 1)] != moveColor)
                {
                    return false;
                }
            }

            //Check no ball will be lost
            if (!IsPositionValid(move.RangeX + movementX, move.RangeY + movementY))
            {
                return false;
            }

            //Check destination is available
            int opponentStrength = 0;
            if (rangeIncrementVectorX == movementX
                && rangeIncrementVectorY == movementY)
            {
                //Arrow
                int i = 1;
                while (IsPositionValid(move.RangeX + rangeIncrementVectorX * i, move.RangeY + rangeIncrementVectorY * i)
                    && BoardSpace[move.RangeX + rangeIncrementVectorX * i, move.RangeY + rangeIncrementVectorY * i] == opponentColor)
                {
                    i++;
                }
                if (IsPositionValid(move.RangeX + rangeIncrementVectorX * i, move.RangeY + rangeIncrementVectorY * i)
                    && BoardSpace[move.RangeX + rangeIncrementVectorX * i, move.RangeY + rangeIncrementVectorY * i] == moveColor)
                {
                    return false;
                }
                opponentStrength = i - 1;
            }
            else
            {
                //Inline
                for (int i = 0; i <= rangeVectorLength; i++)
                {
                    if (BoardSpace[move.EndX + rangeIncrementVectorX * (i + 1), move.EndY + rangeIncrementVectorY * (i + 1)] != null)
                    {
                        return false;
                    }
                }
            }

            //Check sumito is strong enough
            if (opponentStrength > 0 && rangeVectorLength < opponentStrength)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ensure the position is on the board
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>True if position is on the grid</returns>
        public static bool IsPositionValid(int x, int y)
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

        /// <summary>
        /// Checks the movement direction against Abalone rules. Only 6 directions possible
        /// </summary>
        /// <param name="vectX"></param>
        /// <param name="vectY"></param>
        /// <returns>True if the move vector is valid</returns>
        private static bool IsVectorValid(int vectX, int vectY)
        {
            return Math.Abs(vectX - vectY) < 2 && Math.Abs(vectX + vectY) <= 2 && Math.Abs(vectX + vectY) > 0;
        }

        /// <summary>
        /// Checks the range of balls to move is valid in the rules.
        /// You can only move 1, 2 or 3 balls that are on a straight line and adjascent in the game coordinates.
        /// </summary>
        /// <param name="vectX"></param>
        /// <param name="vectY"></param>
        /// <returns></returns>
        private static bool IsRangeValid(int vectX, int vectY)
        {
            if ((vectX == vectY || vectX == 0 || vectY == 0) && Math.Abs(vectX) < 3 && Math.Abs(vectY) < 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}