namespace VSoftDiceGame.Scenes;

class RootScreen : ScreenObject
{
    private ScreenSurface _mainSurface;

    public RootScreen()
    {
        _mainSurface = new ScreenSurface(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);

        _mainSurface.Fill(Color.Black, Color.Black, 0);

        _mainSurface.DrawBox(new Rectangle(0, 0, GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT), 
                            ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThin));

        _mainSurface.Print(2, 0, " [ KOŚCI 2026 - VSOFT EDITION ] ", Color.Yellow, Color.Black);

        Children.Add(_mainSurface);
    }
}
