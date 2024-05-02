namespace SnakeGame.App.Handlers.KeyPress;

public class DownKeyPressHandler(GameState gameState) : KeyPressHandler(gameState)
{
    public override void HandleKeyPress() 
        => _gameState.SetSnakeDirection(Direction.Down);
}