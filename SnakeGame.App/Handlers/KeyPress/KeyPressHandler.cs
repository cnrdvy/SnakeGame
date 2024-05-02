namespace SnakeGame.App.Handlers.KeyPress;

public abstract class KeyPressHandler(GameState gameState)
{
    protected GameState _gameState = gameState;

    public abstract void HandleKeyPress();
}
