namespace DiceGame.Scenes;

using DiceGame.Components;
using DiceGame;

class RootScreen : ScreenObject
{
    private ScreenSurface _mainSurface;
    private ScreenSurface _diceTray;

    public RootScreen()
    {
        _mainSurface = new ScreenSurface(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT);
        _mainSurface.Fill(Color.Black, Color.Black, 0);

        _diceTray = new ScreenSurface(53, 10);
        _diceTray.Position = new Point(0, 5); 
        _diceTray.Font = Game.Instance.Fonts["Cheepicus12"];

        for (int i = 0; i < 5; i++)
        {
            bool isHeld = (i == 2);
            DiceRenderer.Draw(_diceTray.Surface, 2 + (i * 10), 0, i + 1, isHeld);
        }

        _mainSurface.DrawBox(new Rectangle(0, 0, GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT), 
                            ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThin, new ColoredGlyph(Color.White, Color.Black)));

        _mainSurface.Print(2, 0, " [ DICE GAME 2026 ] ", Color.Yellow, Color.Black);

        Children.Add(_mainSurface);
        Children.Add(_diceTray);
    }
}
