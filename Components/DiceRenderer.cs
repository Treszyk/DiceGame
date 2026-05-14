namespace DiceGame.Components;

public static class DiceRenderer
{
    private static readonly int[][] _valuePatterns = new int[][]
    {
        new int[] { 3 },
        new int[] { 0, 6 },
        new int[] { 0, 3, 6 },
        new int[] { 0, 1, 5, 6 },
        new int[] { 0, 1, 3, 5, 6 },
        new int[] { 0, 1, 2, 4, 5, 6 }
    };

    public static void Draw(ICellSurface surface, int x, int y, int value, bool isHeld, bool isActive = true)
    {
        Color faceColor, dotColor, borderColor;

        if (!isActive)
        {
            faceColor = new Color(30, 30, 30);
            dotColor = new Color(70, 70, 70);
            borderColor = new Color(60, 60, 60);
        }
        else if (isHeld)
        {
            faceColor = Color.Black;
            dotColor = Color.White;
            borderColor = Color.White;
        }
        else
        {
            faceColor = Color.White;
            dotColor = Color.Black;
            borderColor = Color.Black;
        }

        surface.Fill(new Rectangle(x, y, 9, 9), dotColor, faceColor, 0);

        if (isHeld)
        {
            surface.DrawBox(new Rectangle(x, y, 9, 9), 
                ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThick, new ColoredGlyph(borderColor, faceColor)));
        }

        char dotGlyph = (char)219;
        
        Point[] dots = new Point[]
        {
            new Point(1, 1), new Point(7, 1),
            new Point(1, 4), new Point(4, 4), new Point(7, 4),
            new Point(1, 7), new Point(7, 7)
        };

        if (value >= 1 && value <= 6)
        {
            foreach (int index in _valuePatterns[value - 1])
            {
                var pos = dots[index];
                surface.SetGlyph(x + pos.X, y + pos.Y, dotGlyph, dotColor, faceColor);
            }
        }
    }

    public static void DrawSmall(ICellSurface surface, int x, int y, int value, bool isActive = true)
    {
        Color faceColor = isActive ? Color.White : new Color(30, 30, 30);
        Color dotColor = isActive ? Color.Black : new Color(70, 70, 70);

        surface.Fill(new Rectangle(x, y, 3, 3), dotColor, faceColor, 0);

        char dotGlyph = (char)254;
        
        Point[] dots = new Point[]
        {
            new Point(0, 0), new Point(2, 0),
            new Point(0, 1), new Point(1, 1), new Point(2, 1),
            new Point(0, 2), new Point(2, 2)
        };

        if (value >= 1 && value <= 6)
        {
            foreach (int index in _valuePatterns[value - 1])
            {
                var pos = dots[index];
                surface.SetGlyph(x + pos.X, y + pos.Y, dotGlyph, dotColor, faceColor);
            }
        }
    }
}
