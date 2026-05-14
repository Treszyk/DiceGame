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
    private readonly Rectangle _rollButtonBounds = new Rectangle(2, 1, 19, 3);

    public ControlsView(int width, int height, GameHand hand) : base(width, height, Theme.NeonGreen)
    {
        _hand = hand;
        _hand.OnHandChanged += Redraw;
        UseMouse = true;
        Redraw();
    }

    public override void Update(System.TimeSpan delta)
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
                    _blinkTimer = System.TimeSpan.Zero;
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
        string btnText = " [ RZUT KOSCMI ] ";

        if (_showNegative)
        {
            Surface.Fill(_rollButtonBounds, Theme.Black, btnColor, 0);
            Surface.Print(3, 2, btnText, Theme.Black, btnColor);
        }
        else
        {
            Surface.Print(3, 2, btnText, btnColor, Theme.Black);
        }

        string rightLabel = "[ RZUTY: ";
        int rollsLeft = GameHand.MaxRolls - _hand.RollCount;
        string rightNum = rollsLeft.ToString();
        string rightBracket = " ]";
        
        int startX = Width - rightLabel.Length - rightNum.Length - rightBracket.Length - 4;
        
        Surface.Print(startX, 2, rightLabel, Theme.NeonGreen, Theme.Black);
        Surface.Print(startX + rightLabel.Length, 2, rightNum, Color.Cyan, Theme.Black);
        Surface.Print(startX + rightLabel.Length + rightNum.Length, 2, rightBracket, Theme.NeonGreen, Theme.Black);
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
