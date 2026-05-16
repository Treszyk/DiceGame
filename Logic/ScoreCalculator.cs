using System;
using System.Linq;

namespace DiceGame.Logic;

public static class ScoreCalculator
{
    public static int Calculate(int categoryIndex, Die[] dice)
    {
        if (dice == null || dice.Length != 5) return 0;
        
        int[] values = dice.Select(d => d.Value).ToArray();
        
        switch (categoryIndex)
        {
            case ScoreCategory.Ones: return SumSpecific(values, 1);
            case ScoreCategory.Twos: return SumSpecific(values, 2);
            case ScoreCategory.Threes: return SumSpecific(values, 3);
            case ScoreCategory.Fours: return SumSpecific(values, 4);
            case ScoreCategory.Fives: return SumSpecific(values, 5);
            case ScoreCategory.Sixes: return SumSpecific(values, 6);
            
            case ScoreCategory.ThreeOfAKind: return HasOfAKind(values, 3) ? values.Sum() : 0;
            case ScoreCategory.FourOfAKind: return HasOfAKind(values, 4) ? values.Sum() : 0;
            case ScoreCategory.FullHouse: return IsFullHouse(values) ? 25 : 0;
            case ScoreCategory.SmallStraight: return HasStraight(values, 4) ? 30 : 0;
            case ScoreCategory.LargeStraight: return HasStraight(values, 5) ? 40 : 0;
            case ScoreCategory.King: return HasOfAKind(values, 5) ? 50 : 0;
            case ScoreCategory.Chance: return values.Sum();
            
            default: return 0;
        }
    }

    public static int CalculateUpperSum(int?[] scores)
    {
        int sum = 0;
        for (int i = ScoreCategory.Ones; i <= ScoreCategory.Sixes; i++)
            sum += scores[i].GetValueOrDefault();
        return sum;
    }

    public static int CalculateBonus(int upperSum) => 
        upperSum >= ScoreCategory.BonusThreshold ? ScoreCategory.BonusValue : 0;

    public static int CalculateLowerSum(int?[] scores)
    {
        int sum = 0;
        for (int i = ScoreCategory.ThreeOfAKind; i <= ScoreCategory.Chance; i++)
            sum += scores[i].GetValueOrDefault();
        return sum;
    }

    private static int SumSpecific(int[] values, int target) => values.Where(v => v == target).Sum();

    private static bool HasOfAKind(int[] values, int count)
    {
        return values.GroupBy(value => value).Any(group => group.Count() >= count);
    }

    private static bool IsFullHouse(int[] values)
    {
        var groups = values.GroupBy(value => value).ToList();
        if (groups.Count == 2)
        {
            return (groups[0].Count() == 2 && groups[1].Count() == 3) ||
                   (groups[0].Count() == 3 && groups[1].Count() == 2);
        }
        return false;
    }

    private static bool HasStraight(int[] values, int length)
    {
        var distinctSorted = values.Distinct().OrderBy(v => v).ToList();
        int currentRun = 1;
        int maxRun = 1;

        for (int i = 1; i < distinctSorted.Count; i++)
        {
            if (distinctSorted[i] == distinctSorted[i - 1] + 1)
            {
                currentRun++;
                if (currentRun > maxRun) maxRun = currentRun;
            }
            else
            {
                currentRun = 1;
            }
        }

        return maxRun >= length;
    }
}
