namespace SnakeGame.App;

public class Position(int row, int column)
{
    public int Row { get; set; } = row;
    public int Column { get; set; } = column;

    public override bool Equals(object? obj) =>
        obj is Position position 
        && Row == position.Row 
        && Column == position.Column;

    public override int GetHashCode() => HashCode.Combine(Row, Column);

    public Position Translate(Direction direction) 
        => new(Row + direction.RowOffset, Column + direction.ColumnOffset);

    public static bool operator ==(Position? left, Position? right) 
        => EqualityComparer<Position>.Default.Equals(left, right);

    public static bool operator !=(Position? left, Position? right) => !(left == right);
}
