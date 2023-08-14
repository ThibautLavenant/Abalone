using System;
using System.Collections.Generic;
using System.Linq;
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
            Grid.SetRow(MoveArrows, e.x);
            Grid.SetColumn(MoveArrows, e.y * 2 + 4 - e.x);
            _startCoordinate = new Coordinates(e.x, e.y);
            MoveArrows.Visibility = Visibility.Visible;
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
                    while (_game.Board.BoardSpace[rangeX + moveX, rangeY + moveY] != null)
                    {
                        rangeX += moveX;
                        rangeY += moveY;
                    }
                    _game.Board.ExecuteMove(new Move()
                    {
                        StartX = _startCoordinate.x,
                        StartY = _startCoordinate.y,
                        EndX = endX,
                        EndY = endY,
                        RangeX = rangeX,
                        RangeY = rangeY,
                    });
                    await App.Current.Dispatcher.BeginInvoke(() => DisplayGame(_game));
                });
            }
        }
    }
}
