using SadConsole;
using SadRogue.Primitives;

namespace DiceGame.Components.Views;

public class ScoreboardView : BasePanel
{
    private static readonly string[] Categories = {
        "JEDYNKI", "DWOJKI", "TROJKI", "CZWORKI", "PIATKI", "SZOSTKI",
        "SUMA", "PREMIA", "SUMA (G)",
        "3 JEDNAKOWE", "4 JEDNAKOWE", "FULL",
        "MALY STRIT", "DUZY STRIT", "KROL", "SZANSA", "SUMA (D)",
        "RAZEM"
    };

    private const int TopSectionCount = 9;
    private const int BottomSectionCount = 8;

    public ScoreboardView(int playerCount, int height)
        : base(GameSettings.LabelWidth + (playerCount * GameSettings.ColWidth) + 1, height, Theme.Amber)
    {
        for (int i = 0; i < Categories.Length; i++)
        {
            int y = GetRowY(i);

            Surface.Print(2, y, Categories[i], Theme.Amber);

            if (i < Categories.Length - 1 && i != 8)
                Surface.DrawLine(new Point(1, y + 1), new Point(Width - 2, y + 1), 196, Theme.Amber);

            for (int p = 0; p < playerCount; p++)
            {
                int x = GameSettings.LabelWidth + (p * GameSettings.ColWidth);
                if (p == 0 && i < 6)
                    PrintCentered(x + 1, GameSettings.ColWidth - 1, y, "12", Theme.NeonGreen);
            }
        }

        for (int x = 1; x < Width - 1; x++)
            Surface.SetBackground(x, 22, Theme.Amber);

        for (int p = 0; p <= playerCount; p++)
        {
            int x = GameSettings.LabelWidth + (p * GameSettings.ColWidth);
            if (x < Width)
                Surface.DrawLine(new Point(x, 1), new Point(x, Height - 2), 179, Theme.Amber);

            if (p < playerCount)
                PrintCentered(x + 1, GameSettings.ColWidth - 1, 2, $"GRACZ  {p + 1}", Theme.Amber);
        }
    }

    private int GetRowY(int index)
    {
        if (index < TopSectionCount)
            return 4 + (index * 2);
        
        return 24 + ((index - TopSectionCount) * 2);
    }
}
