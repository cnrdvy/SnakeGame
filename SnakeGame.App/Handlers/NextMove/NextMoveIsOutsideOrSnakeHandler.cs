namespace SnakeGame.App.Handlers.NextMove;

public class NextMoveIsOutsideOrSnakeHandler(GameState gameState) : NextMoveHandler(gameState)
{
    public override void HandleNextMove(Position newHeadPosition) => _gameState.IsGameOver = true;
}