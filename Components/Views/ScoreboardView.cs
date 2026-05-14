using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using DiceGame.Logic;

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

    private static readonly bool[] IsPlayable = {
        true, true, true, true, true, true,
        false, false, false,
        true, true, true,
        true, true, true, true, false,
        false
    };

    private const int TopSectionCount = 9;
    
    private readonly GameHand _hand;
    private readonly PlayerState[] _players;
    private int _hoveredCategory = -1;

    private bool _isGameOver = false;
    private System.Collections.Generic.List<int> _winners = new();

    public int ActivePlayerIndex { get; set; } = 0;
    public event System.Action? OnScoreLocked;

    public ScoreboardView(PlayerState[] players, GameHand hand, int height)
        : base(GameSettings.LabelWidth + (players.Length * GameSettings.ColWidth) + 1, height, Theme.Amber)
    {
        _players = players;
        _hand = hand;
        UseMouse = true;
        _hand.OnHandChanged += Redraw;
        Redraw();
    }

    public void SetGameOver(System.Collections.Generic.List<int> winners)
    {
        _isGameOver = true;
        _winners = winners;
        Redraw();
    }

    public void Redraw()
    {
        Surface.Clear();
        DrawBorder();
        
        DrawGridAndLabels();
        DrawHeaders();
        DrawAllPlayerScores();
    }

    private void DrawGridAndLabels()
    {
        for (int i = 0; i < Categories.Length; i++)
        {
            int y = GetRowY(i);
            Surface.Print(2, y, Categories[i], Theme.Amber);

            if (i < Categories.Length - 1 && i != 8)
                Surface.DrawLine(new Point(1, y + 1), new Point(Width - 2, y + 1), 196, Theme.Amber);
        }

        for (int x = 1; x < Width - 1; x++)
            Surface.SetBackground(x, 22, Theme.Amber);

        for (int p = 0; p <= _players.Length; p++)
        {
            int x = GameSettings.LabelWidth + (p * GameSettings.ColWidth);
            if (x < Width)
                Surface.DrawLine(new Point(x, 1), new Point(x, Height - 2), 179, Theme.Amber);
        }
    }

    private void DrawHeaders()
    {
        for (int p = 0; p < _players.Length; p++)
        {
            int x = GameSettings.LabelWidth + (p * GameSettings.ColWidth);
            Color headerColor = Theme.Amber;
            
            if (_isGameOver)
            {
                headerColor = _winners.Contains(p) ? Theme.NeonGreen : Color.Red;
            }
            else
            {
                headerColor = p == ActivePlayerIndex ? Theme.White : Theme.Amber;
            }
            
            PrintCentered(x + 1, GameSettings.ColWidth - 1, 2, $"GRACZ  {p + 1}", headerColor);
        }
    }

    private void DrawAllPlayerScores()
    {
        for (int i = 0; i < Categories.Length; i++)
        {
            int y = GetRowY(i);
            for (int p = 0; p < _players.Length; p++)
            {
                int x = GameSettings.LabelWidth + (p * GameSettings.ColWidth);
                DrawCellScore(p, i, x, y);
            }
        }
    }

    private void DrawCellScore(int playerIndex, int categoryIndex, int x, int y)
    {
        if (_players[playerIndex].Scores[categoryIndex].HasValue)
        {
            string text = _players[playerIndex].Scores[categoryIndex].GetValueOrDefault().ToString().PadLeft(2, '0');
            Color scoreColor = Theme.NeonGreen;
            
            if (_isGameOver)
            {
                scoreColor = _winners.Contains(playerIndex) ? Theme.NeonGreen : Color.Red;
            }
            
            PrintCentered(x + 1, GameSettings.ColWidth - 1, y, text, scoreColor);
        }
        else if (!_isGameOver && playerIndex == ActivePlayerIndex && IsPlayable[categoryIndex] && _hand.RollCount > 0)
        {
            int ghostScore = ScoreCalculator.Calculate(categoryIndex, _hand.Dice);
            string text = ghostScore.ToString().PadLeft(2, '0');
            
            if (categoryIndex == _hoveredCategory)
            {
                Surface.Fill(new Rectangle(x + 1, y, GameSettings.ColWidth - 1, 1), Theme.Black, Color.Cyan, 0);
                PrintCentered(x + 1, GameSettings.ColWidth - 1, y, text, Theme.Black, Color.Cyan);
            }
            else
            {
                PrintCentered(x + 1, GameSettings.ColWidth - 1, y, text, Color.Cyan, Theme.Black);
            }
        }
    }

    private int GetRowY(int index)
    {
        if (index < TopSectionCount)
            return 4 + (index * 2);
        
        return 24 + ((index - TopSectionCount) * 2);
    }

    private int? GetCategoryAtY(int y)
    {
        for (int i = 0; i < Categories.Length; i++)
        {
            if (GetRowY(i) == y) return i;
        }
        return null;
    }

    public override bool ProcessMouse(MouseScreenObjectState state)
    {
        if (_isGameOver) return base.ProcessMouse(state);

        Point pos = state.CellPosition;
        int activeColumnXStart = GameSettings.LabelWidth + (ActivePlayerIndex * GameSettings.ColWidth);
        int activeColumnXEnd = activeColumnXStart + GameSettings.ColWidth;

        int previousHover = _hoveredCategory;
        _hoveredCategory = -1;

        if (pos.X > activeColumnXStart && pos.X < activeColumnXEnd && _hand.RollCount > 0)
        {
            int? cat = GetCategoryAtY(pos.Y);
            if (cat.HasValue && IsPlayable[cat.Value] && !_players[ActivePlayerIndex].Scores[cat.Value].HasValue)
            {
                _hoveredCategory = cat.Value;

                if (state.Mouse.LeftClicked)
                {
                    int ghostScore = ScoreCalculator.Calculate(_hoveredCategory, _hand.Dice);
                    _players[ActivePlayerIndex].LockScore(_hoveredCategory, ghostScore);
                    
                    _hand.Reset();
                    _hoveredCategory = -1;
                    OnScoreLocked?.Invoke();
                }
            }
        }

        if (previousHover != _hoveredCategory)
            Redraw();

        return base.ProcessMouse(state);
    }
}
