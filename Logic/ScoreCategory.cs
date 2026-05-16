namespace DiceGame.Logic;

public static class ScoreCategory
{
    public const int Ones = 0;
    public const int Twos = 1;
    public const int Threes = 2;
    public const int Fours = 3;
    public const int Fives = 4;
    public const int Sixes = 5;
    
    public const int UpperSum = 6;
    public const int Bonus = 7;
    public const int UpperTotal = 8;

    public const int ThreeOfAKind = 9;
    public const int FourOfAKind = 10;
    public const int FullHouse = 11;
    public const int SmallStraight = 12;
    public const int LargeStraight = 13;
    public const int King = 14;
    public const int Chance = 15;

    public const int LowerTotal = 16;
    public const int GrandTotal = 17;

    // the email said 62, but the assignment PDF said 63, so I chose to keep 63
    // and document it here
    public const int BonusThreshold = 63;
    public const int BonusValue = 35;

    public static readonly int[] PlayableCategories = { 
        Ones, Twos, Threes, Fours, Fives, Sixes, 
        ThreeOfAKind, FourOfAKind, FullHouse, 
        SmallStraight, LargeStraight, King, 
        Chance 
    };
}
