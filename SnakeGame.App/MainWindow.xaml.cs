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
        #region Constructor & DI

        private readonly int _rows = 15, _columns = 15;
        private readonly Image[,] _gridImages;
        private readonly Dictionary<GridValueEnumeration, ImageSource> _gridValueToImage 
            = new()
        {
            { GridValueEnumeration.Empty, Images.Empty },
            { GridValueEnumeration.Snake, Images.Body },
            { GridValueEnumeration.Food, Images.Food }
        };
        private GameState gameState;
        private bool isGameRunning;
        private readonly Dictionary<Direction, int> directionToRotateHead = new()
        {
            { Direction.Up, 0 },
            { Direction.Right, 90 },
            { Direction.Down, 180 },
            { Direction.Left, 270 }
        };

        public MainWindow()
        {
            InitializeComponent();
            _gridImages = SetupGrid();
            gameState = new GameState(_rows, _columns);
        }

        #endregion

        #region Events

        private void Window_KeyDown(object sender, KeyEventArgs e) 
            => gameState.HandleKeyPress(e.Key);

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Overlay.Visibility == Visibility.Visible)
                e.Handled = true;

            if (!isGameRunning)
            {
                isGameRunning = true;
                await RunGame();
                isGameRunning = false;
            }
        }

        #endregion

        #region Private Methods

        private async Task GameLoop()
        {
            while (!gameState.IsGameOver)
            {
                await Task.Delay(100);
                gameState.HandleNextMove();
                Draw();
            }
        }

        private void Draw()
        {
            DrawGrid();
            DrawSnakeHead();
            ScoreText.Text = $"SCORE {gameState.Score}";
        }

        private void DrawGrid()
        {
            for (var row = 0; row < _rows; row++)
            {
                for (var column = 0; column < _columns; column++)
                {
                    var gridValue = gameState.Grid[row, column];
                    _gridImages[row, column].Source = _gridValueToImage[gridValue];
                    _gridImages[row, column].RenderTransform = Transform.Identity;
                }
            }
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
                    var image = new Image 
                    { 
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5)
                    };
                    images[row, column] = image;
                    GameGrid.Children.Add(image);
                }
            }

            return images;
        }

        private async Task RunGame()
        {
            Draw();
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            await GameLoop();
            await ShowGameOver();
            gameState = new GameState(_rows, _columns);
        }

        private async Task ShowCountDown()
        {
            for (var countDown = 3; countDown >= 1; countDown--)
            {
                OverlayText.Text = countDown.ToString();
                await Task.Delay(500);
            }
        }

        private async Task ShowGameOver()
        {
            await DrawDeadSnake();
            await Task.Delay(1000);
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "PRESS ANY KEY TO START";
        }

        private void DrawSnakeHead()
        {
            var headPosition = gameState.GetSnakeHeadPosition();
            var headImage = _gridImages[headPosition.Row, headPosition.Column];
            headImage.Source = Images.Head;

            var amountToRotateHead = directionToRotateHead[gameState.Direction];
            headImage.RenderTransform = new RotateTransform(amountToRotateHead);
        }

        private async Task DrawDeadSnake()
        {
            var snakePositions = gameState.GetSnakePositions().ToList();
            for (var i = 0; i < snakePositions.Count; i++) 
            {
                var snakePosition = snakePositions[i];

                if (i == 0)
                {
                    var deadSnakeHeadImage
                        = _gridImages[snakePosition.Row, snakePosition.Column];
                    deadSnakeHeadImage.Source = Images.DeadHead;

                    var amountToRotateHead = directionToRotateHead[gameState.Direction];
                    deadSnakeHeadImage.RenderTransform = new RotateTransform(
                        amountToRotateHead);
                }
                else
                {
                    var deadSnakeImage
                        = _gridImages[snakePosition.Row, snakePosition.Column];
                    deadSnakeImage.Source = Images.DeadBody;
                }

                await Task.Delay(50);
            }
        }

        #endregion
    }
}