using DiceGame.Logic.Models;
using DiceGame.Logic.Scoring;
using DiceGame.Components.Core;
using DiceGame.Components.Views.Gameplay;
using DiceGame.Components.Views.GameOver;

namespace DiceGame.Scenes;

public class RootScreen : ScreenObject, IDisposable
{
    private HeaderView _header;
    private DiceTrayView _diceTray;
    private ControlsView _controls;
    private ScoreboardView _scoreboard;
    
    private readonly GameSession _session;
    public event Action? OnQuitToMenuRequested;

    public RootScreen(int playerCount, Action? onQuit)
    {
        if (onQuit != null) OnQuitToMenuRequested += onQuit;
        _session = new GameSession(playerCount);

        int p = GameSettings.Padding;
        int lw = GameSettings.LeftWidth;

        _header = new HeaderView(lw, GameSettings.HeaderHeight);
        _header.OnQuitToMenu += () => OnQuitToMenuRequested?.Invoke();
        
        _diceTray = new DiceTrayView(lw, GameSettings.DiceTrayHeight, _session.Hand);
        _controls = new ControlsView(lw, GameSettings.ControlsHeight, _session.Hand);
        _scoreboard = new ScoreboardView(_session, GameSettings.ScoreboardHeight);

        _header.Position = new Point(p, p);
        _diceTray.Position = new Point(p, p + GameSettings.HeaderHeight);
        _controls.Position = new Point(p, p + GameSettings.HeaderHeight + GameSettings.DiceTrayHeight);
        _scoreboard.Position = new Point(p + lw, p);
        
        _session.OnTurnAdvanced += SyncSessionToUi;
        _session.OnGameOver += TriggerGameOver;

        Children.Add(_header);
        Children.Add(_diceTray);
        Children.Add(_controls);
        Children.Add(_scoreboard);

        IsFocused = true;
    }

    public void Dispose()
    {
        _session.OnTurnAdvanced -= SyncSessionToUi;
        _session.OnGameOver -= TriggerGameOver;
        
        _header.Dispose();
        _diceTray.Dispose();
        _controls.Dispose();
        _scoreboard.Dispose();
    }


    private void SyncSessionToUi()
    {
        _scoreboard.ActivePlayerIndex = _session.ActivePlayerIndex;
        _scoreboard.Redraw();
        _header.SetActivePlayer(_session.ActivePlayerIndex);
    }

    private void TriggerGameOver()
    {
        SoundUtility.PlayGameEnd();
        
        var winners = _session.GetWinners(out int maxScore);
        _scoreboard.SetGameOver(winners);

        Children.Remove(_diceTray);
        Children.Remove(_controls);

        var gameOverBanner = new GameOverBannerView(GameSettings.LeftWidth, GameSettings.DiceTrayHeight, winners, maxScore);
        gameOverBanner.Position = _diceTray.Position;

        var gameOverControls = new GameOverControlsView(GameSettings.LeftWidth, GameSettings.ControlsHeight);
        gameOverControls.Position = _controls.Position;

        gameOverControls.OnPlayAgain += () => 
        {
            SadConsole.Game.Instance.Screen = new RootScreen(_session.PlayerCount, OnQuitToMenuRequested);
        };
        
        gameOverControls.OnMainMenu += () => 
        {
            OnQuitToMenuRequested?.Invoke();
        };

        Children.Add(gameOverBanner);
        Children.Add(gameOverControls);
    }

    public override bool ProcessKeyboard(SadConsole.Input.Keyboard keyboard)
    {
        return base.ProcessKeyboard(keyboard);
    }
}
