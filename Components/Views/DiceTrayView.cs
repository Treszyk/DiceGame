using SadConsole;
using SadRogue.Primitives;

namespace DiceGame.Components.Views;

public class DiceTrayView : BasePanel
{
    private const int DiceSize = 9;
    private const int DiceGap = 3;

    public DiceTrayView(int width, int height) : base(width, height, Theme.NeonGreen)
    {
        int row1Count = 3;
        int row2Count = 2;

        int row1Total = row1Count * DiceSize + (row1Count - 1) * DiceGap;
        int row2Total = row2Count * DiceSize + (row2Count - 1) * DiceGap;

        int row1X = (width - row1Total) / 2;
        int row2X = (width - row2Total) / 2;

        int row1Y = 2;
        int row2Y = row1Y + DiceSize + 4;

        for (int i = 0; i < row1Count; i++)
        {
            int x = row1X + i * (DiceSize + DiceGap);
            DiceRenderer.Draw(Surface, x, row1Y, i + 1, i == 2);
            DrawHoldButton(x, row1Y + DiceSize + 1, i == 2);
        }

        for (int i = 0; i < row2Count; i++)
        {
            int x = row2X + i * (DiceSize + DiceGap);
            int diceIndex = row1Count + i;
            DiceRenderer.Draw(Surface, x, row2Y, diceIndex + 1, false);
            DrawHoldButton(x, row2Y + DiceSize + 1, false);
        }

        int hintY = row2Y + DiceSize + 4;
        PrintCentered(0, width, hintY, "BIEZACA REKA: TROJKA", Theme.Amber);
    }

    private void DrawHoldButton(int x, int y, bool isHeld)
    {
        if (isHeld)
            Surface.Print(x, y, " TRZYMAJ ", Theme.Black, Theme.NeonGreen);
        else
            Surface.Print(x, y, " TRZYMAJ ", new Color(80, 80, 80), Theme.Black);
    }
}
