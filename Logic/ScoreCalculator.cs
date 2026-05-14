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
            case 0: return SumSpecific(values, 1);
            case 1: return SumSpecific(values, 2);
            case 2: return SumSpecific(values, 3);
            case 3: return SumSpecific(values, 4);
            case 4: return SumSpecific(values, 5);
            case 5: return SumSpecific(values, 6);
            
            case 9: return HasOfAKind(values, 3) ? values.Sum() : 0;
            case 10: return HasOfAKind(values, 4) ? values.Sum() : 0;
            case 11: return IsFullHouse(values) ? 25 : 0;
            case 12: return HasStraight(values, 4) ? 30 : 0;
            case 13: return HasStraight(values, 5) ? 40 : 0;
            case 14: return HasOfAKind(values, 5) ? 50 : 0;
            case 15: return values.Sum();
            
            default: return 0;
        }
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
