using SadConsole;
using SadRogue.Primitives;

namespace DiceGame.Components.Views;

public class ControlsView : BasePanel
{
    public ControlsView(int width, int height) : base(width, height, Theme.NeonGreen)
    {
        Surface.Print(4, 2, "[ RZUT KOSCMI ]", Theme.White, Theme.Black);
        string rightText = "[ RZUTY: 2 ]";
        Surface.Print(width - rightText.Length - 4, 2, rightText, Theme.NeonGreen, Theme.Black);
    }
}
