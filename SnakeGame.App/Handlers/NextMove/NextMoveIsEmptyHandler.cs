namespace SnakeGame.App.Handlers.NextMove;

public class NextMoveIsEmptyHandler(GameState gameState) : NextMoveHandler(gameState)
{
    public override void HandleNextMove(Position newHeadPosition)
    {
        _gameState.RemoveSnakeTailPosition();
        _gameState.SetSnakeHeadPosition(newHeadPosition);
    }
}