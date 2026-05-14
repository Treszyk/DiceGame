using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using DiceGame.Logic;
using System;

namespace DiceGame.Components.Views;

public class ControlsView : BasePanel
{
    private readonly GameHand _hand;
    private bool _isHovering = false;
    private bool _showNegative = false;
    private TimeSpan _blinkTimer = TimeSpan.Zero;
    private readonly TimeSpan _blinkInterval = TimeSpan.FromSeconds(0.5);
    private readonly Rectangle _rollButtonBounds = new Rectangle(4, 2, 15, 1);

    public ControlsView(int width, int height, GameHand hand) : base(width, height, Theme.NeonGreen)
    {
        _hand = hand;
        _hand.OnHandChanged += Redraw;
        UseMouse = true;
        Redraw();
    }

    public override void Update(TimeSpan delta)
    {
        base.Update(delta);

        if (_hand.CanRoll)
        {
            if (_isHovering)
            {
                if (!_showNegative)
                {
                    _showNegative = true;
                    Redraw();
                }
            }
            else
            {
                _blinkTimer += delta;
                if (_blinkTimer >= _blinkInterval)
                {
                    _blinkTimer = TimeSpan.Zero;
                    _showNegative = !_showNegative;
                    Redraw();
                }
            }
        }
        else if (_showNegative)
        {
            _showNegative = false;
            Redraw();
        }
    }

    public void Redraw()
    {
        Surface.Clear();
        DrawBorder();

        Color btnColor = _hand.CanRoll ? Theme.White : new Color(80, 80, 80);
        string btnText = "[ RZUT KOSCMI ]";

        if (_showNegative)
            Surface.Print(_rollButtonBounds.X, _rollButtonBounds.Y, btnText, Theme.Black, btnColor);
        else
            Surface.Print(_rollButtonBounds.X, _rollButtonBounds.Y, btnText, btnColor, Theme.Black);

        string rightText = $"[ RZUTY: {GameHand.MaxRolls - _hand.RollCount} ]";
        Surface.Print(Width - rightText.Length - 4, 2, rightText, Theme.NeonGreen, Theme.Black);
    }

    public override bool ProcessMouse(MouseScreenObjectState state)
    {
        Point pos = state.CellPosition;
        bool wasHovering = _isHovering;
        _isHovering = _rollButtonBounds.Contains(pos);

        if (wasHovering != _isHovering)
            Redraw();

        if (state.Mouse.LeftClicked && _isHovering && _hand.CanRoll)
        {
            _hand.Roll();
            return true;
        }
        
        return base.ProcessMouse(state);
    }
}
