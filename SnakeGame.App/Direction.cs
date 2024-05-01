namespace SnakeGame.App;

public class Direction(int rowOffset, int columnOffset)
{
    public readonly static Direction Left = new(0, -1);
    public readonly static Direction Right = new(0, 1);
    public readonly static Direction Up = new(-1, 0);
    public readonly static Direction Down = new(1, 0);

    public int RowOffset { get; set; } = rowOffset;
    public int ColumnOffset { get; set; } = columnOffset;

    public override bool Equals(object? obj) 
        => obj is Direction direction 
        && RowOffset == direction.RowOffset 
        && ColumnOffset == direction.ColumnOffset;

    public override int GetHashCode() => HashCode.Combine(RowOffset, ColumnOffset);

    public Direction Opposite() => new(-RowOffset, -ColumnOffset);

    public static bool operator ==(Direction? left, Direction? right) 
        => EqualityComparer<Direction>.Default.Equals(left, right);

    public static bool operator !=(Direction? left, Direction? right) => !(left == right);
}
