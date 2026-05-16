using DiceGame.Logic.Models;
using DiceGame.Components.Core;

namespace DiceGame.Components.Views.Gameplay;

public class DiceTrayView : BasePanel
{
    private const int DiceSize = 9;
    private const int DiceGap = 3;
    private readonly GameHand _hand;

    public DiceTrayView(int width, int height, GameHand hand) : base(width, height, Theme.NeonGreen)
    {
        _hand = hand;
        _hand.OnHandChanged += Redraw;
        UseMouse = true;
        Redraw();
    }

    public override void Dispose()
    {
        _hand.OnHandChanged -= Redraw;
        base.Dispose();
    }

    private Point GetDiePosition(int index)
    {
        int row1Count = 3;
        int row2Count = 2;
        int row1Total = row1Count * DiceSize + (row1Count - 1) * DiceGap;
        int row2Total = row2Count * DiceSize + (row2Count - 1) * DiceGap;
        
        int row1Y = 2;
        int row2Y = row1Y + DiceSize + 4;

        if (index < row1Count)
            return new Point((Width - row1Total) / 2 + index * (DiceSize + DiceGap), row1Y);
        
        int i = index - row1Count;
        return new Point((Width - row2Total) / 2 + i * (DiceSize + DiceGap), row2Y);
    }

    private Rectangle GetDieHitbox(int index)
    {
        Point pos = GetDiePosition(index);
        return new Rectangle(pos.X, pos.Y, DiceSize, DiceSize + 2);
    }

    public void Redraw()
    {
        Surface.Clear();
        DrawBorder();

        bool isActive = _hand.RollCount > 0;

        for (int i = 0; i < 5; i++)
        {
            Point pos = GetDiePosition(i);
            DiceRenderer.Draw(Surface, pos.X, pos.Y, _hand.Dice[i].Value, _hand.Dice[i].IsHeld, isActive);
            DrawHoldButton(pos.X, pos.Y + DiceSize + 1, _hand.Dice[i].IsHeld);
        }

        int lastRowY = GetDiePosition(4).Y;
        
        string hint1 = "";
        string hint2 = "";

        if (_hand.RollCount == 0)
        {
            hint1 = "RZUC KOSCMI ABY ZACZAC!";
        }
        else if (_hand.RollCount < GameHand.MaxRolls)
        {
            hint1 = "WYBIERZ KATEGORIE NA TABLICY WYNIKOW!";
            hint2 = "ALBO RZUC KOSCMI PONOWNIE!";
        }
        else
        {
            hint1 = "WYBIERZ KATEGORIE NA TABLICY WYNIKOW!";
        }

        PrintCentered(0, Width, lastRowY + DiceSize + 4, hint1, Theme.Amber);
        if (hint2 != "")
            PrintCentered(0, Width, lastRowY + DiceSize + 5, hint2, Theme.Amber);
    }

    private void DrawHoldButton(int x, int y, bool isHeld)
    {
        if (isHeld)
            Surface.Print(x, y, " TRZYMAJ ", Theme.Black, Theme.NeonGreen);
        else
            Surface.Print(x, y, " TRZYMAJ ", Theme.Gray, Theme.Black);
    }

    public override bool ProcessMouse(MouseScreenObjectState state)
    {
        if (state.Mouse.LeftClicked)
        {
            if (_hand.RollCount == 0)
                return base.ProcessMouse(state);

            for (int i = 0; i < 5; i++)
            {
                if (GetDieHitbox(i).Contains(state.CellPosition))
                {
                    _hand.ToggleHold(i);
                    return true;
                }
            }
        }
        return base.ProcessMouse(state);
    }
}
