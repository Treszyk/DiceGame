using System;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace DiceGame.Logic;

public static class SoundUtility
{
    private static SoundEffect? _bgm;
    private static SoundEffectInstance? _bgmInstance;
    private static SoundEffect? _hover;
    private static SoundEffect? _select;
    private static SoundEffect? _roll;
    private static SoundEffect? _lock;
    private static SoundEffect? _inactive;
    private static SoundEffect? _gameEnd;
    private static bool _initialized = false;

    public static void Initialize()
    {
        if (_initialized) return;
        _initialized = true;

        try
        {
            string[] searchPaths = {
                "Assets/Sounds",
                "../../../Assets/Sounds",
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Sounds"),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Sounds")
            };

            string? root = null;
            foreach (var path in searchPaths)
            {
                string fullPath = Path.GetFullPath(path);
                if (Directory.Exists(fullPath)) { root = fullPath; break; }
            }

            if (root != null)
            {
                string sPath = Path.Combine(root, "select.wav");
                string rPath = Path.Combine(root, "roll.wav");
                string hPath = Path.Combine(root, "hover.wav");
                string lPath = Path.Combine(root, "lock.wav");
                string iPath = Path.Combine(root, "inactive_btn.wav");
                string mPath = Path.Combine(root, "bit_shift.wav");
                string gePath = Path.Combine(root, "game_end.wav");

                if (File.Exists(hPath)) _hover = SoundEffect.FromFile(hPath);
                if (File.Exists(sPath)) _select = SoundEffect.FromFile(sPath);
                if (File.Exists(rPath)) _roll = SoundEffect.FromFile(rPath);
                if (File.Exists(lPath)) _lock = SoundEffect.FromFile(lPath);
                if (File.Exists(iPath)) _inactive = SoundEffect.FromFile(iPath);
                if (File.Exists(gePath)) _gameEnd = SoundEffect.FromFile(gePath);
                
                if (File.Exists(mPath)) 
                {
                    _bgm = SoundEffect.FromFile(mPath);
                    _bgmInstance = _bgm.CreateInstance();
                    _bgmInstance.IsLooped = true;
                    _bgmInstance.Volume = 0.15f;
                }
            }
        }
        catch { }
    }

    public static void PlayGameEnd()
    {
        _gameEnd?.Play();
    }

    public static void PlayBGM()
    {
        if (_bgmInstance != null && _bgmInstance.State != SoundState.Playing)
        {
            _bgmInstance.Play();
        }
    }

    public static void PlayInactive()
    {
        _inactive?.Play();
    }

    public static void PlayHover()
    {
        _hover?.Play();
    }

    public static void PlaySelect()
    {
        _select?.Play();
    }

    public static void PlayDiceRoll()
    {
        _roll?.Play();
    }

    public static void PlayLock()
    {
        _lock?.Play();
    }
}
