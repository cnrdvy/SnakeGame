using SnakeGame.App.Handlers.KeyPress;
using SnakeGame.App.Handlers.NextMove;
using System.Windows.Input;

namespace SnakeGame.App;

public class GameState
{
    #region Constructor & DI

    private readonly LinkedList<Position> _snakePositions = new();
    private readonly Random _random = new();
    private readonly Dictionary<Key, KeyPressHandler> _keyHandlers;
    private readonly Dictionary<GridValueEnumeration, NextMoveHandler> _nextMoveHandlers;

    public GameState(int rows, int columns)
    {
        _keyHandlers = new Dictionary<Key, KeyPressHandler>
        {
            { Key.Left, new LeftKeyPressHandler(this) },
            { Key.Right,new RightKeyPressHandler(this) },
            { Key.Up, new UpKeyPressHandler(this) },
            { Key.Down, new DownKeyPressHandler(this) }
        };

        _nextMoveHandlers = new Dictionary<GridValueEnumeration, NextMoveHandler>
        {
            { GridValueEnumeration.Outside, new NextMoveIsOutsideOrSnakeHandler(this) },
            { GridValueEnumeration.Snake, new NextMoveIsOutsideOrSnakeHandler(this) },
            { GridValueEnumeration.Empty, new NextMoveIsEmptyHandler(this) },
            { GridValueEnumeration.Food, new NextMoveIsFoodHandler(this) }
        };

        Rows = rows;
        Columns = columns;
        Grid = new GridValueEnumeration[rows, columns];

        SetSnakePosition();
        SetFoodPosition();
    }

    #endregion

    #region Properties

    public int Rows { get; }
    public int Columns { get; }
    public GridValueEnumeration[,] Grid { get; }
    public Direction Direction { get; private set; } = Direction.Right;
    public int Score { get; set; }
    public bool IsGameOver { get; set; }

    #endregion

    #region Public Methods

    public void HandleKeyPress(Key key)
    {
        if (IsGameOver)
            return;

        if (_keyHandlers.TryGetValue(key, out var keyHandler))
            keyHandler.HandleKeyPress();
    }

    public Position GetSnakeHeadPosition() => _snakePositions.First.Value;

    public Position GetSnakeTailPosition() => _snakePositions.Last.Value;

    public IEnumerable<Position> GetSnakePositions() => _snakePositions;

    public void SetSnakeDirection(Direction direction)
    {
        Direction = direction;
    }

    public void HandleNextMove()
    {
        Position newHeadPosition = GetSnakeHeadPosition().Translate(Direction);
        GridValueEnumeration nextMove = GetSnakeNextMove(newHeadPosition);

        if (_nextMoveHandlers.TryGetValue(nextMove, out var nextMoveHandler))
            nextMoveHandler.HandleNextMove(newHeadPosition);
    }

    #endregion

    #region Private Methods

    private void SetSnakePosition()
    {
        int middleRow = Rows / 2;

        for (var column = 1; column <= 3; column++)
        {
            Grid[middleRow, column] = GridValueEnumeration.Snake;
            _snakePositions.AddFirst(new Position(middleRow, column));
        }
    }

    private List<Position> GetEmptyPositions()
    {
        var emptyPositions = new List<Position>();

        for (var row = 0; row < Rows; row++)
        {
            for (var column = 0; column < Columns; column++)
            {
                if (Grid[row, column] == GridValueEnumeration.Empty)
                    emptyPositions.Add(new Position(row, column));
            }
        }

        return emptyPositions;
    }

    public void SetFoodPosition()
    {
        var emptyPositions = GetEmptyPositions();
        if (emptyPositions.Count == 0)
            return;

        var foodPosition = emptyPositions[_random.Next(emptyPositions.Count)];
        Grid[foodPosition.Row, foodPosition.Column] = GridValueEnumeration.Food;
    }

    public void SetSnakeHeadPosition(Position position)
    {
        _snakePositions.AddFirst(position);
        Grid[position.Row, position.Column] = GridValueEnumeration.Snake;
    }

    public void RemoveSnakeTailPosition()
    {
        Position tailPosition = _snakePositions.Last.Value;
        Grid[tailPosition.Row, tailPosition.Column] = GridValueEnumeration.Empty;
        _snakePositions.RemoveLast();
    }

    private bool IsSnakeOutsideGrid(Position position) 
        => position.Row < 0 
        || position.Row >= Rows 
        || position.Column < 0 
        || position.Column >= Columns;

    private GridValueEnumeration GetSnakeNextMove(Position newHeadPosition)
    {
        if (IsSnakeOutsideGrid(newHeadPosition))
            return GridValueEnumeration.Outside;

        if (newHeadPosition == _snakePositions.Last.Value)
            return GridValueEnumeration.Empty;

        return Grid[newHeadPosition.Row, newHeadPosition.Column];
    }

    #endregion
}
