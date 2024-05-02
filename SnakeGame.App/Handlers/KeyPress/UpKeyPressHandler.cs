namespace SnakeGame.App.Handlers.KeyPress;

public class UpKeyPressHandler(GameState gameState) : KeyPressHandler(gameState)
{
    public override void HandleKeyPress() => _gameState.SetSnakeDirection(Direction.Up);
}
