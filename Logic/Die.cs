namespace DiceGame.Logic;

using System;

public class Die
{
    private static readonly Random _random = new Random();
    
    public int Value { get; private set; }
    public bool IsHeld { get; set; }

    public Die()
    {
        Value = 1;
        IsHeld = false;
    }

    public void Roll()
    {
        if (!IsHeld)
            Value = _random.Next(1, 7);
    }

    public void Reset()
    {
        IsHeld = false;
        Value = 1;
    }
}
