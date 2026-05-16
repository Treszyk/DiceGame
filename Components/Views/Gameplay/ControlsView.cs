using DiceGame.Logic.Models;
using DiceGame.Components.Core;

namespace DiceGame.Components.Views.Gameplay;

public class ControlsView : BasePanel
{
    private readonly GameHand _hand;
    private bool _isHovering = false;
    private readonly BlinkState _blink = new();
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
            if (_blink.Update(delta, _isHovering)) Redraw();
        }
        else if (_blink.ShowNegative)
        {
            _blink.Reset();
            Redraw();
        }
    }

    public void Redraw()
    {
        Surface.Clear();
        DrawBorder();

        Color btnColor = _hand.CanRoll ? Theme.White : Theme.Gray;
        string btnText = " [ RZUT KOSCMI ] ";

        if (_blink.ShowNegative)
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
        Surface.Print(startX + rightLabel.Length, 2, rightNum, Theme.Cyan, Theme.Black);
        Surface.Print(startX + rightLabel.Length + rightNum.Length, 2, rightBracket, Theme.NeonGreen, Theme.Black);
    }

    public override bool ProcessMouse(MouseScreenObjectState state)
    {
        Point pos = state.CellPosition;
        bool wasHovering = _isHovering;
        _isHovering = _rollButtonBounds.Contains(pos);

        if (wasHovering != _isHovering)
        {
            if (_isHovering && _hand.CanRoll) SoundUtility.PlayHover();
            Redraw();
        }

        if (state.Mouse.LeftClicked && _isHovering)
        {
            if (_hand.CanRoll)
            {
                SoundUtility.PlayRoll();
                _hand.Roll();
            }
            else
            {
                SoundUtility.PlayInactive();
            }
            return true;
        }
        
        return base.ProcessMouse(state);
    }
}
