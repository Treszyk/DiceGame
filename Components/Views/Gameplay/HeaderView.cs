using DiceGame.Components.Core;

namespace DiceGame.Components.Views.Gameplay;

public class HeaderView : BasePanel
{
    private int _activePlayer = 0;

    private Rectangle _quitBounds;
    private bool _isHoveringQuit = false;

    public System.Action OnQuitToMenu = delegate { };

    public HeaderView(int width, int height) : base(width, height, Theme.NeonGreen)
    {
        UseMouse = true;
        Redraw();
    }

    public void SetActivePlayer(int index)
    {
        _activePlayer = index;
        Redraw();
    }

    private void Redraw()
    {
        Surface.Clear();
        DrawBorder();

        string prefix = "TURA GRACZA ";
        string num = (_activePlayer + 1).ToString();
        
        Surface.Print(2, 2, prefix, Theme.NeonGreen, Theme.Black);
        Surface.Print(2 + prefix.Length, 2, num, Color.Cyan, Theme.Black);

        string rightText = " [ ZAKONCZ GRE ] ";
        int startX = Width - rightText.Length - 2;
        _quitBounds = new Rectangle(startX, 1, rightText.Length, 3);

        Color btnColor = _isHoveringQuit ? Theme.Black : Theme.Amber;
        Color btnBg = _isHoveringQuit ? Theme.Amber : Theme.Black;

        Surface.Fill(_quitBounds, btnColor, btnBg, 0);
        Surface.Print(startX, 2, rightText, btnColor, btnBg);
    }

    public override bool ProcessMouse(SadConsole.Input.MouseScreenObjectState state)
    {
        bool wasHovering = _isHoveringQuit;
        _isHoveringQuit = _quitBounds.Contains(state.CellPosition);

        if (_isHoveringQuit != wasHovering)
        {
            if (_isHoveringQuit) SoundUtility.PlayHover();
            Redraw();
        }

        if (_isHoveringQuit && state.Mouse.LeftClicked)
        {
            SoundUtility.PlaySelect();
            OnQuitToMenu?.Invoke();
            return true;
        }

        return base.ProcessMouse(state);
    }
}
