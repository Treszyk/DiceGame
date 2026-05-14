namespace DiceGame;

public static class GameSettings
{
    public const int PlayerCount = 3;
    public const int Padding = 1;

    public const int LeftWidth = 51;
    public const int HeaderHeight = 5;
    public const int ControlsHeight = 5;

    public const int LabelWidth = 14;
    public const int ColWidth = 13;
    public const int ScoreboardHeight = 42;

    public static int ScoreboardWidth => LabelWidth + (PlayerCount * ColWidth) + 1;
    public static int DiceTrayHeight => ScoreboardHeight - HeaderHeight - ControlsHeight;
    public static int TotalWidth => (Padding * 2) + LeftWidth + ScoreboardWidth;
    public static int TotalHeight => (Padding * 2) + ScoreboardHeight;
}
