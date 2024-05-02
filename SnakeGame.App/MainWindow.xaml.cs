using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SnakeGame.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int _rows = 15, _columns = 15;
        private readonly Image[,] _gridImages;
        private readonly Dictionary<GridValueEnumeration, ImageSource> _gridValueToImage 
            = new()
        {
            { GridValueEnumeration.Empty, Images.Empty },
            { GridValueEnumeration.Snake, Images.Body },
            { GridValueEnumeration.Food, Images.Food }
        };
        private readonly GameState _gameState;

        public MainWindow()
        {
            InitializeComponent();
            _gridImages = SetupGrid();
            _gameState = new GameState(_rows, _columns);
        }

        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[_rows, _columns];
            GameGrid.Rows = _rows;
            GameGrid.Columns = _columns;

            for (var row = 0; row < _rows; row++)
            {
                for (var column = 0; column < _columns; column++)
                {
                    var image = new Image { Source = Images.Empty };
                    images[row, column] = image;
                    GameGrid.Children.Add(image);
                }
            }

            return images;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Draw();
            await GameLoop();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) 
            => _gameState.HandleKeyPress(e.Key);

        private async Task GameLoop()
        {
            while (!_gameState.IsGameOver)
            {
                await Task.Delay(100);
                _gameState.HandleNextMove();
                Draw();
            }
        }

        private void Draw()
        {
            DrawGrid();
            ScoreText.Text = $"SCORE {_gameState.Score}";
        }

        private void DrawGrid()
        {
            for (var row = 0; row < _rows; row++)
            {
                for (var column = 0; column < _columns; column++)
                {
                    var gridValue = _gameState.Grid[row, column];
                    _gridImages[row, column].Source = _gridValueToImage[gridValue];
                }
            }
        }
    }
}