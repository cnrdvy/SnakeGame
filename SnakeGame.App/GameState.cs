namespace SnakeGame.App;

public class GameState
{
    #region Constructor & DI

    private readonly LinkedList<Position> _snakePositions = new();
    private readonly Random _random = new();

    public GameState(int rows, int columns)
    {
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
    public int Score { get; private set; }
    public bool IsGameOver { get; private set; }

    #endregion

    #region Public Methods

    public Position GetSnakeHeadPosition() => _snakePositions.First.Value;

    public Position GetSnakeTailPosition() => _snakePositions.Last.Value;

    public IEnumerable<Position> GetSnakePositions() => _snakePositions;

    public void ChangeSnakeDirection(Direction direction)
    {
        Direction = direction;
    }

    public void MoveSnake()
    {
        var newHeadPosition = GetSnakeHeadPosition().Translate(Direction);
        var collisionPrediction = HandleSnakeCollision(newHeadPosition);

        switch (collisionPrediction)
        {
            case GridValueEnumeration.Outside:
            case GridValueEnumeration.Snake:
                IsGameOver = true;
                break;

            case GridValueEnumeration.Empty:
                RemoveSnakeTailPosition();
                SetSnakeHeadPosition(newHeadPosition);
                break;

            case GridValueEnumeration.Food:
                SetSnakeHeadPosition(newHeadPosition);
                SetFoodPosition();

                Score++;
                break;
        }
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

    private void SetFoodPosition()
    {
        var emptyPositions = GetEmptyPositions();
        if (emptyPositions.Count == 0)
            return;

        var foodPosition = emptyPositions[_random.Next(emptyPositions.Count)];
        Grid[foodPosition.Row, foodPosition.Column] = GridValueEnumeration.Food;
    }

    private void SetSnakeHeadPosition(Position position)
    {
        _snakePositions.AddFirst(position);
        Grid[position.Row, position.Column] = GridValueEnumeration.Snake;
    }

    private void RemoveSnakeTailPosition()
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

    private GridValueEnumeration HandleSnakeCollision(Position newHeadPosition)
    {
        if (IsSnakeOutsideGrid(newHeadPosition))
            return GridValueEnumeration.Outside;

        if (newHeadPosition == _snakePositions.Last.Value)
            return GridValueEnumeration.Empty;

        return Grid[newHeadPosition.Row, newHeadPosition.Column];
    }

    #endregion
}
