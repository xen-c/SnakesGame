using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snakes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {//Creating Grids

        private readonly Dictionary<GridValue, ImageSource> gridValToImage = new()
        {
            {GridValue.Empty, Images.Empty },
            {GridValue.Snake, Images.Body },
            {GridValue.Food, Images.Food }
        };

        //Adding images
        private readonly int rows = 15, col = 15;
        private readonly Image[,] gridImages;
        private GameState gameState;
        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetUpGrid();
            gameState = new GameState(rows, col);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DrawGrid();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private Image[,] SetUpGrid()
        {
            Image[,] images = new Image[rows, col];
            GameGrid.Rows= rows;
            GameGrid.Columns= col;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty
                    };

                    images[r,c] = image;
                    GameGrid.Children.Add(image);
                }
            }

            return images;
        }

        private void DrawGrid()
        {
            for (int r = 0; r < rows; r++)
            {
                for(int c = 0;c < col; c++)
                {
                    GridValue gridVal = gameState.Grid[r,c];
                    gridImages[r, c].Source = gridValToImage[gridVal];
                }
            }
        }
    }
}