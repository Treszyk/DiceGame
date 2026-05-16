using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace DiceGame.Components.Core;

public static class SoundUtility
{
    private static readonly Dictionary<string, SoundEffect> _sounds = new();

    public static void Initialize()
    {
        string soundDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Sounds");
        
        foreach (var file in Directory.GetFiles(soundDir, "*.wav"))
        {
            string name = Path.GetFileNameWithoutExtension(file);
            using var stream = File.OpenRead(file);
            _sounds[name] = SoundEffect.FromStream(stream);
        }
    }

    public static void PlayRoll() => Play("roll");
    public static void PlayLock() => Play("lock");
    public static void PlayHover() => Play("hover");
    public static void PlaySelect() => Play("select");
    public static void PlayGameEnd() => Play("game_end");
    public static void PlayInactive() => Play("inactive_btn");
    public static void PlayBGM() => Play("bit_shift");

    private static void Play(string name)
    {
        if (_sounds.TryGetValue(name, out var sound))
            sound.Play();
    }
}
