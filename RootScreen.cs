namespace DiceGame.Scenes;

using DiceGame.Components;
using DiceGame.Components.Views;
using SadConsole;
using SadRogue.Primitives;

public class RootScreen : ScreenObject
{
    private HeaderView _header;
    private DiceTrayView _diceTray;
    private ControlsView _controls;
    private ScoreboardView _scoreboard;

    public RootScreen()
    {
        try 
        { 
            ((dynamic)SadConsole.Game.Instance).ScreenOptions.AllowWindowResize = false;
            var mono = ((dynamic)SadConsole.Game.Instance).MonoGameInstance;
            mono.Window.AllowUserResizing = false;
        } 
        catch { }

        int p = GameSettings.Padding;
        int lw = GameSettings.LeftWidth;

        _header = new HeaderView(lw, GameSettings.HeaderHeight);
        _diceTray = new DiceTrayView(lw, GameSettings.DiceTrayHeight);
        _controls = new ControlsView(lw, GameSettings.ControlsHeight);
        _scoreboard = new ScoreboardView(GameSettings.PlayerCount, GameSettings.ScoreboardHeight);

        _header.Position = new Point(p, p);
        _diceTray.Position = new Point(p, p + GameSettings.HeaderHeight);
        _controls.Position = new Point(p, p + GameSettings.HeaderHeight + GameSettings.DiceTrayHeight);
        _scoreboard.Position = new Point(p + lw, p);

        Children.Add(_header);
        Children.Add(_diceTray);
        Children.Add(_controls);
        Children.Add(_scoreboard);
    }
}
