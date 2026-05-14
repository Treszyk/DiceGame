using SadConsole;
using SadRogue.Primitives;

namespace DiceGame.Components.Views;

public class HeaderView : BasePanel
{
    private int _activePlayer = 0;

    public HeaderView(int width, int height) : base(width, height, Theme.NeonGreen)
    {
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

        string rightText = "[ ZAKONCZ GRE ]";
        Surface.Print(Width - rightText.Length - 2, 2, rightText, Theme.Amber, Theme.Black);
    }
}
