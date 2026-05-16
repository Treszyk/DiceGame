
using Microsoft.Xna.Framework.Audio;

namespace DiceGame.Components.Core;

public static class SoundUtility
{
    private static readonly Dictionary<string, SoundEffect> _sounds = new();

    public static void Initialize()
    {
        string soundDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Sounds");
        
        if (!Directory.Exists(soundDir)) return;

        foreach (var file in Directory.GetFiles(soundDir, "*.wav"))
        {
            string name = Path.GetFileNameWithoutExtension(file);
            using var stream = File.OpenRead(file);
            _sounds[name] = SoundEffect.FromStream(stream);
        }
    }

    private static SoundEffectInstance? _bgmInstance;

    public static void PlayRoll() => Play("roll");
    public static void PlayLock() => Play("lock");
    public static void PlayHover() => Play("hover");
    public static void PlaySelect() => Play("select");
    public static void PlayGameEnd() => Play("game_end");
    public static void PlayInactive() => Play("inactive_btn");
    
    public static void PlayBGM()
    {
        if (_bgmInstance != null) return;
        
        if (_sounds.TryGetValue("bit_shift", out var sound))
        {
            _bgmInstance = sound.CreateInstance();
            _bgmInstance.IsLooped = true;
            _bgmInstance.Volume = 0.3f;
            _bgmInstance.Play();
        }
    }

    private static void Play(string name, float volume = 1.0f)
    {
        if (_sounds.TryGetValue(name, out var sound))
            sound.Play(volume, 0.0f, 0.0f);
    }
}
