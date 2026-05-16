using System.Security.Cryptography;
using DiceGame.Components.Core;

namespace DiceGame.Components.Views.GameOver;

public class GameOverBannerView : BasePanel
{
    private readonly List<int> _winners;
    private readonly int _winningScore;

    private TimeSpan _animTimer = System.TimeSpan.Zero;
    private readonly TimeSpan _animInterval = System.TimeSpan.FromMilliseconds(300);
    private readonly int[] _cornerDiceValues = { 1, 2, 3, 4 };

    public GameOverBannerView(int width, int height, List<int> winners, int winningScore) : base(width, height, Theme.NeonGreen)
    {
        _winners = winners;
        _winningScore = winningScore;
        Redraw();
    }

    public override void Update(System.TimeSpan delta)
    {
        base.Update(delta);
        
        _animTimer += delta;
        if (_animTimer >= _animInterval)
        {
            _animTimer = System.TimeSpan.Zero;
            for (int i = 0; i < 4; i++)
            {
                _cornerDiceValues[i] = RandomNumberGenerator.GetInt32(1, 7);
            }
            Redraw();
        }
    }

    private void Redraw()
    {
        Surface.Clear();
        DrawBorder();

        DiceRenderer.DrawSmall(Surface, 2, 2, _cornerDiceValues[0]);
        DiceRenderer.DrawSmall(Surface, Width - 5, 2, _cornerDiceValues[1]);
        DiceRenderer.DrawSmall(Surface, 2, Height - 5, _cornerDiceValues[2]);
        DiceRenderer.DrawSmall(Surface, Width - 5, Height - 5, _cornerDiceValues[3]);

        int startY = Height / 2 - 6;
        
        string hBorder = "+-----------------------------------------+";
        string vBorder = "|                                         |";
        
        PrintCentered(0, Width, startY, hBorder, Theme.NeonGreen);
        PrintCentered(0, Width, startY + 1, vBorder, Theme.NeonGreen);
        PrintCentered(0, Width, startY + 2, vBorder, Theme.NeonGreen);
        PrintCentered(0, Width, startY + 3, vBorder, Theme.NeonGreen);
        PrintCentered(0, Width, startY + 4, hBorder, Theme.NeonGreen);
        
        PrintCentered(0, Width, startY + 2, "K O N I E C   G R Y !", Theme.White);

        int currentY = startY + 6;

        if (_winners.Count == 1)
        {
            string winnerText = $"WYGRYWA GRACZ {_winners[0] + 1}!";
            PrintCentered(0, Width, currentY, winnerText, Theme.NeonGreen);
            currentY++;
        }
        else
        {
            var names = new List<string>();
            foreach (var w in _winners) names.Add($"GRACZ {w + 1}");
            
            string prefix = "REMIS! WYGRYWAJA: ";
            string allNames = string.Join(", ", names);
            string fullString = prefix + allNames;

            if (fullString.Length <= 45)
            {
                PrintCentered(0, Width, currentY, fullString, Theme.NeonGreen);
                currentY++;
            }
            else
            {
                PrintCentered(0, Width, currentY, prefix, Theme.NeonGreen);
                currentY++;

                string[] words = allNames.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                string currentLine = "";

                foreach (string word in words)
                {
                    if (currentLine.Length + word.Length + 1 > 45)
                    {
                        PrintCentered(0, Width, currentY, currentLine.Trim(), Theme.NeonGreen);
                        currentY++;
                        currentLine = "";
                    }
                    currentLine += word + " ";
                }

                if (currentLine.Trim().Length > 0)
                {
                    PrintCentered(0, Width, currentY, currentLine.Trim(), Theme.NeonGreen);
                    currentY++;
                }
            }
        }
        
        string scoreText = $"Z WYNIKIEM: {_winningScore} PKT";
        PrintCentered(0, Width, currentY + 1, scoreText, Color.Cyan);
    }
}
