using System;
using System.Collections.Generic;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using DiceGame.Components;

namespace DiceGame.Scenes;

public class MainMenuScreen : ScreenSurface
{
    private readonly Rectangle[] _buttonBounds = new Rectangle[4];
    private int _hoveredIndex = -1;

    private readonly List<Point> _borderPath = new List<Point>();
    private readonly int[] _decoDicePathIndex = new int[8];
    private readonly int[] _decoDiceValues = new int[8];
    private TimeSpan _decoTimer = TimeSpan.Zero;
    private readonly TimeSpan _decoInterval = TimeSpan.FromMilliseconds(40);
    private readonly Random _rnd = new Random();

    public Action<int> OnPlayerCountSelected = delegate { };

    public MainMenuScreen() : base(80, GameSettings.TotalHeight)
    {
        UseMouse = true;
        
        for (int x = 0; x <= Width - 3; x++) _borderPath.Add(new Point(x, 0));
        for (int y = 1; y <= Height - 4; y++) _borderPath.Add(new Point(Width - 3, y));
        for (int x = Width - 3; x >= 0; x--) _borderPath.Add(new Point(x, Height - 3));
        for (int y = Height - 4; y >= 1; y--) _borderPath.Add(new Point(0, y));

        for (int i = 0; i < 8; i++)
        {
            _decoDicePathIndex[i] = (i * _borderPath.Count / 8) % _borderPath.Count;
            _decoDiceValues[i] = _rnd.Next(1, 7);
        }

        int startY = Height / 2 - 2;
        int btnWidth = 25;
        int startX = (Width - btnWidth) / 2;

        for (int i = 0; i < 4; i++)
        {
            _buttonBounds[i] = new Rectangle(startX, startY + (i * 4), btnWidth, 3);
        }

        Redraw();
    }

    public override void Update(TimeSpan delta)
    {
        base.Update(delta);
        _decoTimer += delta;
        if (_decoTimer >= _decoInterval)
        {
            _decoTimer = TimeSpan.Zero;
            for (int i = 0; i < 8; i++)
            {
                _decoDicePathIndex[i] = (_decoDicePathIndex[i] + 1) % _borderPath.Count;
                if (_rnd.NextDouble() < 0.1) _decoDiceValues[i] = _rnd.Next(1, 7);
            }
            Redraw();
        }
    }

    private void Redraw()
    {
        Surface.Clear();

        for (int i = 0; i < 8; i++)
        {
            Point p = _borderPath[_decoDicePathIndex[i]];
            DiceRenderer.DrawSmall(Surface, p.X, p.Y, _decoDiceValues[i]);
        }

        string[] asciiArt = {
            @"  ___  ___ ___ ___    ___   _   __  __ ___ ",
            @" |   \|_ _/ __| __|  / __| /_\ |  \/  | __|",
            @" | |) || | (__| _|  | (_ |/ _ \| |\/| | _| ",
            @" |___/|___\___|___|  \___/_/ \_\_|  |_|___|"
        };

        int logoY = Height / 2 - 10;
        for (int i = 0; i < asciiArt.Length; i++)
        {
            PrintCentered(logoY + i, asciiArt[i], Theme.NeonGreen);
        }

        PrintCentered(logoY + 5, "WYBIERZ LICZBE GRACZY:", Color.Cyan);

        for (int i = 0; i < 4; i++)
        {
            bool isHovered = _hoveredIndex == i;
            string text;
            Color btnColor;
            Color btnBg;

            if (i < 3)
            {
                text = $"[ {i + 2} GRACZY ]";
                btnColor = isHovered ? Theme.Black : Theme.White;
                btnBg = isHovered ? Theme.White : Theme.Black;
            }
            else
            {
                text = "[ WYJDZ Z GRY ]";
                btnColor = isHovered ? Theme.Black : Theme.Amber;
                btnBg = isHovered ? Theme.Amber : Theme.Black;
            }
            
            Surface.Fill(_buttonBounds[i], btnColor, btnBg, 0);
            
            int textX = _buttonBounds[i].X + (_buttonBounds[i].Width - text.Length) / 2;
            int textY = _buttonBounds[i].Y + 1;
            Surface.Print(textX, textY, text, btnColor, btnBg);
        }
    }

    private void PrintCentered(int y, string text, Color color)
    {
        int x = (Width - text.Length) / 2;
        Surface.Print(x, y, text, color);
    }

    public override bool ProcessMouse(MouseScreenObjectState state)
    {
        int prevHovered = _hoveredIndex;
        _hoveredIndex = -1;

        for (int i = 0; i < 4; i++)
        {
            if (_buttonBounds[i].Contains(state.CellPosition))
            {
                _hoveredIndex = i;
                break;
            }
        }

        if (prevHovered != _hoveredIndex)
        {
            Redraw();
        }

        if (_hoveredIndex != -1 && state.Mouse.LeftClicked)
        {
            if (_hoveredIndex < 3)
            {
                OnPlayerCountSelected?.Invoke(_hoveredIndex + 2);
            }
            else
            {
                System.Environment.Exit(0);
            }
            return true;
        }

        return base.ProcessMouse(state);
    }
}
