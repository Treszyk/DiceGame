namespace DiceGame.Logic;

public class GameHand
{
    public Die[] Dice { get; private set; }
    public int RollCount { get; private set; }
    public const int MaxRolls = 3;

    public GameHand()
    {
        Dice = new Die[5];
        for (int i = 0; i < 5; i++)
            Dice[i] = new Die();
        
        RollCount = 0;
    }

    public void Roll()
    {
        if (RollCount < MaxRolls)
        {
            foreach (var die in Dice)
                die.Roll();
            
            RollCount++;
        }
    }

    public void Reset()
    {
        foreach (var die in Dice)
            die.Reset();
        
        RollCount = 0;
    }

    public bool CanRoll => RollCount < MaxRolls;
}
