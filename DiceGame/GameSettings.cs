namespace DiceGame;

public static class GameSettings
{
    public const int MaxPlayers = 4;
    public const int Padding = 1;

    public const int LeftWidth = 51;
    public const int HeaderHeight = 5;
    public const int ControlsHeight = 5;

    public const int LabelWidth = 14;
    public const int ColWidth = 13;
    public const int ScoreboardHeight = 42;

    public static int GetScoreboardWidth(int playerCount) => LabelWidth + (playerCount * ColWidth) + 1;
    public static int DiceTrayHeight => ScoreboardHeight - HeaderHeight - ControlsHeight;
    public static int GetTotalWidth(int playerCount) => (Padding * 2) + LeftWidth + GetScoreboardWidth(playerCount);
    
    public static int MaxTotalWidth => GetTotalWidth(MaxPlayers);
    public static int TotalHeight => (Padding * 2) + ScoreboardHeight;
}
