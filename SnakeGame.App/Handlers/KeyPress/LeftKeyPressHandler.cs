namespace SnakeGame.App.Handlers.KeyPress;

public class LeftKeyPressHandler(GameState gameState) : KeyPressHandler(gameState)
{
    public override void HandleKeyPress() 
        => _gameState.SetSnakeDirection(Direction.Left);
}