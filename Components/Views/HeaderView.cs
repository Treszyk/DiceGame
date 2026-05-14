using SadConsole;
using SadRogue.Primitives;

namespace DiceGame.Components.Views;

public class HeaderView : BasePanel
{
    public HeaderView(int width, int height) : base(width, height, Theme.NeonGreen)
    {
        string leftText = "TURA GRACZA 1";
        string rightText = "[ ZAKONCZ GRE ]";
        Surface.Print(2, 2, leftText, Theme.NeonGreen, Theme.Black);
        Surface.Print(width - rightText.Length - 2, 2, rightText, Theme.Amber, Theme.Black);
    }
}
