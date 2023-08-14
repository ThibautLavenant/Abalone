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
    /// Interaction logic for BoardCell.xaml
    /// </summary>
    public partial class BoardCell : UserControl
    {
        private int coordX;
        private int coordY;

        public int CoordX { 
            get => coordX; 
            set {
                coordX = value;
                SetCoordValue();
            }
        }

        public int CoordY { 
            get => coordY; 
            set
            {
                coordY = value;
                SetCoordValue();
            }
        }
        public EventHandler<Coordinates> CellClicked { get; set; }

        public BoardCell()
        {
            InitializeComponent();
        }

        public void SetPlayer(EPlayer? player)
        {
            if (player == EPlayer.BLACK)
            {
                CellEllipse.Fill = Brushes.Black;
                Coordinate.Foreground = Brushes.White;
            }
            else if (player == EPlayer.WHITE)
            {
                CellEllipse.Fill = Brushes.LightGray;
                Coordinate.Foreground = Brushes.Black;
            }
            else
            {
                CellEllipse.Fill = Brushes.Transparent;
                Coordinate.Foreground = Brushes.Black;
            }
        }

        private void SetCoordValue()
        {
            Coordinate.Text = $"{CoordX},{CoordY}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CellClicked?.Invoke(this, new Coordinates(CoordX, CoordY));
        }
    }

    public struct Coordinates
    {
        public int x, y;

        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
