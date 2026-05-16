using System.Security.Cryptography;

namespace DiceGame.Logic.Models;

public class Die
{
    public int Value { get; private set; }
    public bool IsHeld { get; set; }

    public Die(int value = 1)
    {
        Value = value;
        IsHeld = false;
    }

    public void Roll()
    {
        if (!IsHeld)
            Value = RandomNumberGenerator.GetInt32(1, 7);
    }

    public void Reset()
    {
        Value = 1;
        IsHeld = false;
    }
}
