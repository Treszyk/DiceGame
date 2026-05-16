using DiceGame.Logic.Scoring;

namespace DiceGame.Logic.Models;

public class PlayerState
{
    public int?[] Scores { get; private set; }

    public PlayerState()
    {
        Scores = new int?[18];
    }

    public void LockScore(int categoryIndex, int score)
    {
        Scores[categoryIndex] = score;
        UpdateSums();
    }

    private void UpdateSums()
    {
        int upperSum = ScoreCalculator.CalculateUpperSum(Scores);
        Scores[ScoreCategory.UpperSum] = upperSum;
        
        int bonus = ScoreCalculator.CalculateBonus(upperSum);
        Scores[ScoreCategory.Bonus] = bonus;

        Scores[ScoreCategory.UpperTotal] = upperSum + bonus;

        int lowerSum = ScoreCalculator.CalculateLowerSum(Scores);
        Scores[ScoreCategory.LowerTotal] = lowerSum;

        Scores[ScoreCategory.GrandTotal] = Scores[ScoreCategory.UpperTotal].GetValueOrDefault() + lowerSum;
    }

    public bool HasEmptyCategories()
    {
        return ScoreCategory.PlayableCategories.Any(i => !Scores[i].HasValue);
    }
}
