using DiceGame.Logic.Models;
using DiceGame.Logic.Scoring;
using DiceGame.Components.Core;
using DiceGame.Components.Views.Gameplay;
using DiceGame.Components.Views.GameOver;

namespace DiceGame.Scenes;

public class RootScreen : ScreenObject
{
    private HeaderView _header;
    private DiceTrayView _diceTray;
    private ControlsView _controls;
    private ScoreboardView _scoreboard;
    
    private readonly GameSession _session;
    public System.Action OnQuitToMenuRequested = delegate { };

    public RootScreen(int playerCount, System.Action onQuit)
    {
        OnQuitToMenuRequested = onQuit;
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
        /*
        if (keyboard.IsKeyPressed(SadConsole.Input.Keys.F1))
        {
            DiceGame.Logic.CheatingUtility.ForceWin(_players, 0);
            AdvanceTurn();
            return true;
        }
        if (keyboard.IsKeyPressed(SadConsole.Input.Keys.F2))
        {
            DiceGame.Logic.CheatingUtility.ForceWin(_players, 0, 2);
            AdvanceTurn();
            return true;
        }
        if (keyboard.IsKeyPressed(SadConsole.Input.Keys.F3))
        {
            DiceGame.Logic.CheatingUtility.ForceUpperBonus(_players, 0);
            _scoreboard.Redraw();
            return true;
        }
        if (keyboard.IsKeyPressed(SadConsole.Input.Keys.F4))
        {
            DiceGame.Logic.CheatingUtility.ForceWin(_players, System.Linq.Enumerable.Range(0, _playerCount).ToArray());
            AdvanceTurn();
            return true;
        }
        */

        return base.ProcessKeyboard(keyboard);
    }
}
