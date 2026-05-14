namespace DiceGame.Scenes;

using DiceGame.Components;
using DiceGame.Components.Views;
using DiceGame.Logic;
using SadConsole;
using SadRogue.Primitives;

public class RootScreen : ScreenObject
{
    private HeaderView _header;
    private DiceTrayView _diceTray;
    private ControlsView _controls;
    private ScoreboardView _scoreboard;
    private GameHand _hand;
    private PlayerState[] _players;
    private int _activePlayerIndex = 0;

    public RootScreen()
    {
        try 
        { 
            ((dynamic)SadConsole.Game.Instance).ScreenOptions.AllowWindowResize = false;
            var mono = ((dynamic)SadConsole.Game.Instance).MonoGameInstance;
            mono.Window.AllowUserResizing = false;
        } 
        catch { }

        _hand = new GameHand();
        
        _players = new PlayerState[GameSettings.PlayerCount];
        for (int i = 0; i < GameSettings.PlayerCount; i++)
            _players[i] = new PlayerState();

        int p = GameSettings.Padding;
        int lw = GameSettings.LeftWidth;

        _header = new HeaderView(lw, GameSettings.HeaderHeight);
        _diceTray = new DiceTrayView(lw, GameSettings.DiceTrayHeight, _hand);
        _controls = new ControlsView(lw, GameSettings.ControlsHeight, _hand);
        _scoreboard = new ScoreboardView(_players, _hand, GameSettings.ScoreboardHeight);

        _header.Position = new Point(p, p);
        _diceTray.Position = new Point(p, p + GameSettings.HeaderHeight);
        _controls.Position = new Point(p, p + GameSettings.HeaderHeight + GameSettings.DiceTrayHeight);
        _scoreboard.Position = new Point(p + lw, p);
        _scoreboard.OnScoreLocked += AdvanceTurn;

        Children.Add(_header);
        Children.Add(_diceTray);
        Children.Add(_controls);
        Children.Add(_scoreboard);

        IsFocused = true;
    }

    private void AdvanceTurn()
    {
        _activePlayerIndex++;
        if (_activePlayerIndex >= GameSettings.PlayerCount)
        {
            _activePlayerIndex = 0;
        }

        _scoreboard.ActivePlayerIndex = _activePlayerIndex;
        _scoreboard.Redraw();
        _header.SetActivePlayer(_activePlayerIndex);

        if (System.Linq.Enumerable.All(_players, p => !p.HasEmptyCategories()))
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        int maxScore = -1;
        var winners = new System.Collections.Generic.List<int>();
        
        for (int i = 0; i < _players.Length; i++)
        {
            int score = _players[i].Scores[17].GetValueOrDefault();
            if (score > maxScore)
            {
                maxScore = score;
                winners.Clear();
                winners.Add(i);
            }
            else if (score == maxScore)
            {
                winners.Add(i);
            }
        }

        _scoreboard.SetGameOver(winners);

        Children.Remove(_diceTray);
        Children.Remove(_controls);

        var gameOverBanner = new GameOverBannerView(GameSettings.LeftWidth, GameSettings.DiceTrayHeight, winners, maxScore);
        gameOverBanner.Position = _diceTray.Position;

        var gameOverControls = new GameOverControlsView(GameSettings.LeftWidth, GameSettings.ControlsHeight);
        gameOverControls.Position = _controls.Position;

        gameOverControls.OnPlayAgain += () => 
        {
            SadConsole.Game.Instance.Screen = new RootScreen();
        };
        
        gameOverControls.OnMainMenu += () => 
        {
            System.Environment.Exit(0);
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
            DiceGame.Logic.CheatingUtility.ForceWin(_players, System.Linq.Enumerable.Range(0, GameSettings.PlayerCount).ToArray());
            AdvanceTurn();
            return true;
        }
        */

        return base.ProcessKeyboard(keyboard);
    }
}
