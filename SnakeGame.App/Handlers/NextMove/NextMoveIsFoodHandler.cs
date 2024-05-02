namespace SnakeGame.App.Handlers.NextMove;

public class NextMoveIsFoodHandler(GameState gameState) : NextMoveHandler(gameState)
{
    public override void HandleNextMove(Position newHeadPosition)
    {
        _gameState.SetSnakeHeadPosition(newHeadPosition);
        _gameState.SetFoodPosition();
        _gameState.Score++;
    }
}