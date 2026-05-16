using SadConsole;
using SadRogue.Primitives;

namespace DiceGame.Components.Core;

public abstract class BasePanel : ScreenSurface, IDisposable
{
    protected Color ThemeColor { get; }

    protected BasePanel(int width, int height, Color themeColor) : base(width, height)
    {
        ThemeColor = themeColor;
        Surface.DefaultBackground = Theme.Black;
        Surface.DefaultForeground = themeColor;
        Surface.Clear();
    }

    public new virtual void Dispose()
    {
        base.Dispose();
    }

    protected void DrawBorder()
    {
        Surface.DrawBox(new Rectangle(0, 0, Width, Height), 
            ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThin, new ColoredGlyph(ThemeColor, Theme.Black)));
    }

    protected void PrintCentered(int xStart, int width, int y, string text, Color foreground, Color? background = null)
    {
        int x = xStart + (width - text.Length) / 2;
        Surface.Print(x, y, text, foreground, background ?? Theme.Black);
    }
}
