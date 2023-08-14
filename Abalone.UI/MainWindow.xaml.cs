using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Abalone.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BoardCell[,] boardCells;
        private Game _game = null;

        public MainWindow()
        {
            InitializeComponent();
            MoveArrows.Visibility = Visibility.Collapsed;
            var allCells = Board.Children.OfType<BoardCell>();
            boardCells = new BoardCell[9, 9];
            foreach (var cell in allCells)
            {
                cell.CellClicked += OnCellClicked;
                boardCells[cell.CoordX, cell.CoordY] = cell;
            }

            Task.Run(async () =>
            {
                _game = new Game();
                await App.Current.Dispatcher.BeginInvoke(() => DisplayGame(_game));
            });
        }

        private Coordinates _startCoordinate;
        private void OnCellClicked(object? sender, Coordinates e)
        {
            if (_game.Board.BoardSpace[e.x, e.y] == _game.CurrentPlayer)
            {
                Grid.SetRow(MoveArrows, e.x);
                Grid.SetColumn(MoveArrows, e.y * 2 + 4 - e.x);
                _startCoordinate = new Coordinates(e.x, e.y);
                var move = new Move()
                {
                    StartX = e.x,
                    StartY = e.y,
                };
                MoveArrows.Visibility = Visibility.Visible;
                
                //Arrow A :
                move.EndX = e.x + 1;
                move.EndY = e.y;
                SetArrowMoveRange(move);
                ArrowA.Visibility = _game.IsMoveValid(move) ? Visibility.Visible : Visibility.Collapsed;

                //Arrow B :
                move.EndX = e.x + 1;
                move.EndY = e.y + 1;
                SetArrowMoveRange(move);
                ArrowB.Visibility = _game.IsMoveValid(move) ? Visibility.Visible : Visibility.Collapsed;

                //Arrow C :
                move.EndX = e.x;
                move.EndY = e.y + 1;
                SetArrowMoveRange(move);
                ArrowC.Visibility = _game.IsMoveValid(move) ? Visibility.Visible : Visibility.Collapsed;

                //Arrow D :
                move.EndX = e.x - 1;
                move.EndY = e.y;
                SetArrowMoveRange(move);
                ArrowD.Visibility = _game.IsMoveValid(move) ? Visibility.Visible : Visibility.Collapsed;

                //Arrow E :
                move.EndX = e.x - 1;
                move.EndY = e.y - 1;
                SetArrowMoveRange(move);
                ArrowE.Visibility = _game.IsMoveValid(move) ? Visibility.Visible : Visibility.Collapsed;

                //Arrow F :
                move.EndX = e.x;
                move.EndY = e.y - 1;
                SetArrowMoveRange(move);
                ArrowF.Visibility = _game.IsMoveValid(move) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void SetArrowMoveRange(Move move)
        {
            int currentX = move.StartX;
            int currentY = move.StartY;
            int vectorX = move.EndX - move.StartX;
            int vectorY = move.EndY - move.StartY;
            while (Abalone.Board.IsPositionValid(currentX, currentY) 
                && _game.Board.BoardSpace[move.StartX, move.StartY] == _game.Board.BoardSpace[currentX, currentY])
            {
                currentX += vectorX;
                currentY += vectorY;
            }
            move.RangeX = currentX - vectorX;
            move.RangeY = currentY - vectorY;
        }

        private void DisplayGame(Game game)
        {
            for (int x = 0; x < 9; x++)
            {
                for (int y = Math.Max(0, x - 4); y < Math.Min(9, x + 5); y++)
                {
                    boardCells[x, y].SetPlayer(game.Board.BoardSpace[x, y]);
                }
            }
            TurnColorText.Text = _game.CurrentPlayer.ToString();
            LostBallBlack.Text = _game.LostBalls[EPlayer.BLACK].ToString();
            LostBallWhite.Text = _game.LostBalls[EPlayer.WHITE].ToString();

            if (_game.Winner != null)
            {
                WinnerDisplay.Visibility = Visibility.Visible;
                WinnerColor.Text = _game.Winner.ToString();
            }
            else
            {
                WinnerDisplay.Visibility = Visibility.Collapsed;
                WinnerColor.Text = "";
            }
        }

        private void Arrow_Click(object sender, RoutedEventArgs e)
        {
            if (sender is UIElement elem)
            {
                var gridRow = Grid.GetRow(elem);
                var gridCol = Grid.GetColumn(elem);
                MoveArrows.Visibility = Visibility.Collapsed;
                Task.Run(async () =>
                {
                    var moveX = gridRow - 1;
                    var moveY = ((gridRow + gridCol - 1) / 2) - 1;
                    var endX = _startCoordinate.x + moveX;
                    int endY = _startCoordinate.y + moveY;
                    int rangeX = _startCoordinate.x;
                    int rangeY = _startCoordinate.y;
                    var move = new Move()
                    {
                        StartX = _startCoordinate.x,
                        StartY = _startCoordinate.y,
                        EndX = endX,
                        EndY = endY,
                    };
                    SetArrowMoveRange(move);
                    _game.ExecuteMove(move);
                    await App.Current.Dispatcher.BeginInvoke(() => DisplayGame(_game));
                });
            }
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            MoveArrows.Visibility = Visibility.Collapsed;
            Task.Run(async () =>
            {
                _game = new Game();
                await App.Current.Dispatcher.BeginInvoke(() => DisplayGame(_game));
            });
        }
    }
}
