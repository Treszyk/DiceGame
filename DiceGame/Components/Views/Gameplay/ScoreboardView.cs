using DiceGame.Logic.Models;
using DiceGame.Logic.Scoring;
using DiceGame.Components.Core;

namespace DiceGame.Components.Views.Gameplay;

public class ScoreboardView : BasePanel
{
    private static readonly string[] Categories = {
        "JEDYNKI", "DWOJKI", "TROJKI", "CZWORKI", "PIATKI", "SZOSTKI",
        "SUMA", "PREMIA", "SUMA (G)",
        "3 JEDNAKOWE", "4 JEDNAKOWE", "FULL",
        "MALY STRIT", "DUZY STRIT", "KROL", "SZANSA", "SUMA (D)",
        "RAZEM"
    };


    private const int TopSectionCount = ScoreCategory.ThreeOfAKind;
    
    private readonly GameSession _session;
    private int _hoveredCategory = -1;

    private bool _isGameOver = false;
    private System.Collections.Generic.List<int> _winners = new();

    public int ActivePlayerIndex { get; set; } = 0;

    public ScoreboardView(GameSession session, int height)
        : base(GameSettings.LabelWidth + (session.PlayerCount * GameSettings.ColWidth) + 1, height, Theme.Amber)
    {
        _session = session;
        UseMouse = true;
        _session.Hand.OnHandChanged += Redraw;
        Redraw();
    }

    public override void Dispose()
    {
        _session.Hand.OnHandChanged -= Redraw;
        base.Dispose();
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

            if (i < Categories.Length - 1 && i != ScoreCategory.UpperTotal)
                Surface.DrawLine(new Point(1, y + 1), new Point(Width - 2, y + 1), 196, Theme.Amber);
        }

        int gapY = GetRowY(ScoreCategory.ThreeOfAKind) - 2;
        for (int x = 1; x < Width - 1; x++)
            Surface.SetBackground(x, gapY, Theme.Amber);

        for (int p = 0; p <= _session.PlayerCount; p++)
        {
            int x = GameSettings.LabelWidth + (p * GameSettings.ColWidth);
            if (x < Width)
                Surface.DrawLine(new Point(x, 1), new Point(x, Height - 2), 179, Theme.Amber);
        }
    }

    private void DrawHeaders()
    {
        for (int p = 0; p < _session.PlayerCount; p++)
        {
            int x = GameSettings.LabelWidth + (p * GameSettings.ColWidth);
            Color headerColor = Theme.Amber;
            
            if (_isGameOver)
            {
                headerColor = _winners.Contains(p) ? Theme.NeonGreen : Theme.Red;
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
            for (int p = 0; p < _session.PlayerCount; p++)
            {
                int x = GameSettings.LabelWidth + (p * GameSettings.ColWidth);
                DrawCellScore(p, i, x, y);
            }
        }
    }

    private void DrawCellScore(int playerIndex, int categoryIndex, int x, int y)
    {
        if (_session.Players[playerIndex].Scores[categoryIndex].HasValue)
        {
            string text = _session.Players[playerIndex].Scores[categoryIndex].GetValueOrDefault().ToString().PadLeft(2, '0');
            Color scoreColor = Theme.NeonGreen;
            
            if (_isGameOver)
            {
                scoreColor = _winners.Contains(playerIndex) ? Theme.NeonGreen : Theme.Red;
            }
            
            PrintCentered(x + 1, GameSettings.ColWidth - 1, y, text, scoreColor);
        }
        else if (!_isGameOver && playerIndex == ActivePlayerIndex && ScoreCategory.PlayableCategories.Contains(categoryIndex) && _session.Hand.RollCount > 0)
        {
            int ghostScore = ScoreCalculator.Calculate(categoryIndex, _session.Hand.Dice);
            string text = ghostScore.ToString().PadLeft(2, '0');
            
            if (categoryIndex == _hoveredCategory)
            {
                Surface.Fill(new Rectangle(x + 1, y, GameSettings.ColWidth - 1, 1), Theme.Black, Theme.Cyan, 0);
                PrintCentered(x + 1, GameSettings.ColWidth - 1, y, text, Theme.Black, Theme.Cyan);
            }
            else
            {
                PrintCentered(x + 1, GameSettings.ColWidth - 1, y, text, Theme.Cyan, Theme.Black);
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

        if (pos.X > activeColumnXStart && pos.X < activeColumnXEnd && _session.Hand.RollCount > 0)
        {
            int? cat = GetCategoryAtY(pos.Y);
            if (cat.HasValue && ScoreCategory.PlayableCategories.Contains(cat.Value) && !_session.Players[ActivePlayerIndex].Scores[cat.Value].HasValue)
            {
                _hoveredCategory = cat.Value;
                if (previousHover != _hoveredCategory) SoundUtility.PlayHover();

                if (state.Mouse.LeftClicked)
                {
                    SoundUtility.PlayLock();
                    _session.LockScore(_hoveredCategory);
                    _hoveredCategory = -1;
                }
            }
            else if (state.Mouse.LeftClicked)
            {
                SoundUtility.PlayInactive();
                return true;
            }
        }

        if (previousHover != _hoveredCategory)
            Redraw();

        return base.ProcessMouse(state);
    }
}
