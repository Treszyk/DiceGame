namespace DiceGame.Logic.Models;

public class Die
{
    private static readonly Random _random = new Random();
    public int Value { get; private set; }
    public bool IsHeld { get; set; }

    public Die()
    {
        Reset();
    }

    public void Roll()
    {
        if (!IsHeld)
            Value = _random.Next(1, 7);
    }

    public void Reset()
    {
        Value = 1;
        IsHeld = false;
    }
}
