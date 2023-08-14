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
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public EventHandler<Coordinates> CellClicked { get; set; }

        public BoardCell()
        {
            InitializeComponent();
        }

        public void SetPlayer(Player? player)
        {
            if (player == Player.BLACK)
            {
                CellEllipse.Fill = Brushes.Black;
            }
            else if (player == Player.WHITE)
            {
                CellEllipse.Fill = Brushes.LightGray;
            }
            else
            {
                CellEllipse.Fill = Brushes.Transparent;
            }
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
