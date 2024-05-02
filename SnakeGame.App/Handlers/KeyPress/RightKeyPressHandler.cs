namespace SnakeGame.App.Handlers.KeyPress;

public class RightKeyPressHandler(GameState gameState) : KeyPressHandler(gameState)
{
    public override void HandleKeyPress() 
        => _gameState.SetSnakeDirection(Direction.Right);
}