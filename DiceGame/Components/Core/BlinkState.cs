namespace DiceGame.Components.Core;

public class BlinkState
{
    private TimeSpan _timer = TimeSpan.Zero;
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(0.5);
    public bool ShowNegative { get; private set; }

    public bool Update(TimeSpan delta, bool isHovering)
    {
        if (isHovering)
        {
            if (!ShowNegative)
            {
                ShowNegative = true;
                return true;
            }
            return false;
        }

        _timer += delta;
        if (_timer < _interval) return false;

        _timer = TimeSpan.Zero;
        ShowNegative = !ShowNegative;
        return true;
    }

    public void Reset()
    {
        _timer = TimeSpan.Zero;
        ShowNegative = false;
    }
}
