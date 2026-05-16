using DiceGame.Components.Core;

namespace DiceGame.Components.Views.GameOver;

public class GameOverControlsView : BasePanel
{
    private readonly Rectangle _playAgainBounds = new Rectangle(2, 1, 17, 3);
    private readonly Rectangle _menuBounds;

    private bool _isHoveringPlay = false;
    private bool _isHoveringMenu = false;
    
    private readonly BlinkState _blinkPlay = new();
    private readonly BlinkState _blinkMenu = new();

    public event Action? OnPlayAgain;
    public event Action? OnMainMenu;

    public GameOverControlsView(int width, int height) : base(width, height, Theme.NeonGreen)
    {
        _menuBounds = new Rectangle(width - 21, 1, 19, 3);
        UseMouse = true;
        Redraw();
    }

    public override void Update(TimeSpan delta)
    {
        base.Update(delta);

        bool needsRedraw = false;
        if (_blinkPlay.Update(delta, _isHoveringPlay)) needsRedraw = true;
        if (_blinkMenu.Update(delta, _isHoveringMenu)) needsRedraw = true;

        if (needsRedraw) Redraw();
    }

    public void Redraw()
    {
        Surface.Clear();
        DrawBorder();

        string playText = " [ JESZCZE RAZ ] ";
        if (_blinkPlay.ShowNegative)
        {
            Surface.Fill(_playAgainBounds, Theme.Black, Theme.White, 0);
            Surface.Print(_playAgainBounds.X, 2, playText, Theme.Black, Theme.White);
        }
        else
        {
            Surface.Print(_playAgainBounds.X, 2, playText, Theme.White, Theme.Black);
        }

        string menuText = " [ WYJDZ DO MENU ] ";
        if (_blinkMenu.ShowNegative)
        {
            Surface.Fill(_menuBounds, Theme.Black, Theme.Amber, 0);
            Surface.Print(_menuBounds.X, 2, menuText, Theme.Black, Theme.Amber);
        }
        else
        {
            Surface.Print(_menuBounds.X, 2, menuText, Theme.Amber, Theme.Black);
        }
    }

    public override bool ProcessMouse(MouseScreenObjectState state)
    {
        Point pos = state.CellPosition;
        
        bool wasHoverPlay = _isHoveringPlay;
        bool wasHoverMenu = _isHoveringMenu;

        _isHoveringPlay = _playAgainBounds.Contains(pos);
        _isHoveringMenu = _menuBounds.Contains(pos);

        if (wasHoverPlay != _isHoveringPlay || wasHoverMenu != _isHoveringMenu)
        {
            if (_isHoveringPlay || _isHoveringMenu) SoundUtility.PlayHover();
            Redraw();
        }

        if (state.Mouse.LeftClicked)
        {
            if (_isHoveringPlay)
            {
                SoundUtility.PlaySelect();
                OnPlayAgain?.Invoke();
                return true;
            }
            if (_isHoveringMenu)
            {
                SoundUtility.PlaySelect();
                OnMainMenu?.Invoke();
                return true;
            }
        }

        return base.ProcessMouse(state);
    }
}
