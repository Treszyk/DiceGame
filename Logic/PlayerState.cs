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
}
