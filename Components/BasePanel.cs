using SadConsole;
using SadRogue.Primitives;

namespace DiceGame.Components;

public abstract class BasePanel : ScreenSurface
{
    protected Color BorderColor { get; set; }

    protected BasePanel(int width, int height, Color borderColor) : base(width, height)
    {
        BorderColor = borderColor;
        DrawBorder();
    }

    protected void DrawBorder()
    {
        Surface.Fill(Theme.Black, Theme.Black, 0);
        Surface.DrawBox(new Rectangle(0, 0, Width, Height),
            ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThin, new ColoredGlyph(BorderColor, Theme.Black)));
    }

    protected void PrintCentered(int xStart, int availableWidth, int y, string text, Color color)
    {
        if (text.Length > availableWidth)
            text = text.Substring(0, availableWidth);

        int offset = (availableWidth - text.Length) / 2;
        Surface.Print(xStart + offset, y, text, color, Theme.Black);
    }
}
