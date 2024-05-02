namespace SnakeGame.App.Handlers.NextMove;

public abstract class NextMoveHandler(GameState gameState)
{
    protected GameState _gameState = gameState;

    public abstract void HandleNextMove(Position newHeadPosition);
}