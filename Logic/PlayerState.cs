using System.Linq;

namespace DiceGame.Logic;

public class PlayerState
{
    public int?[] Scores { get; } = new int?[18];

    public void LockScore(int categoryIndex, int score)
    {
        if (categoryIndex < 0 || categoryIndex >= 18 || Scores[categoryIndex] != null) return;
        
        Scores[categoryIndex] = score;
        UpdateSums();
    }

    private void UpdateSums()
    {
        int upperSum = 0;

        for (int i = 0; i <= 5; i++)
        {
            if (Scores[i].HasValue) upperSum += Scores[i].GetValueOrDefault();
        }

        Scores[6] = upperSum;

        // the email said 62, but the assignment PDF said 63, so I chose to keep 63
        // and document it here
        Scores[7] = upperSum >= 63 ? 35 : 0;

        Scores[8] = Scores[6].GetValueOrDefault() + Scores[7].GetValueOrDefault();

        int lowerSum = 0;
        for (int i = 9; i <= 15; i++)
        {
            if (Scores[i].HasValue) lowerSum += Scores[i].GetValueOrDefault();
        }
        
        Scores[16] = lowerSum;

        Scores[17] = Scores[8].GetValueOrDefault() + Scores[16].GetValueOrDefault();
    }

    public bool HasEmptyCategories()
    {
        int[] playableIndices = { 0, 1, 2, 3, 4, 5, 9, 10, 11, 12, 13, 14, 15 };
        return playableIndices.Any(i => !Scores[i].HasValue);
    }
}
