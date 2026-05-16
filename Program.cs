using SadConsole.Configuration;
using DiceGame.Components.Core;
using Microsoft.Xna.Framework;

Settings.WindowTitle = "Dice Game 2026";
Settings.ResizeMode = Settings.WindowResizeOptions.None;
Settings.AllowWindowResize = false;

Builder
    .GetBuilder()
    .SetWindowSizeInPixels(80 * 12, GameSettings.TotalHeight * 12)
    .ConfigureFonts((config, game) => 
    {
        game.LoadFont("Assets/Fonts/Cheepicus_12x12.font");
    })
    .OnStart((sender, game) => 
    {
        game.DefaultFont = game.Fonts["Cheepicus12"];
        SoundUtility.Initialize();
        SoundUtility.PlayBGM();

        game.FrameUpdate += (s, e) => 
        {
            FrameworkDispatcher.Update();
        };

        void SwitchScreen(ScreenObject screen, int width)
        {
            var oldScreen = game.Screen;
            game.ResizeWindow(width, GameSettings.TotalHeight, game.DefaultFont.GetFontSize(IFont.Sizes.One));
            game.Screen = screen;
            screen.IsFocused = true;
            (oldScreen as System.IDisposable)?.Dispose();
        }

        void LoadMainMenu()
        {
            var menu = new DiceGame.Scenes.MainMenuScreen();
            menu.OnPlayerCountSelected = (count) =>
            {
                var root = new DiceGame.Scenes.RootScreen(count, LoadMainMenu);
                SwitchScreen(root, GameSettings.GetTotalWidth(count));
            };
            SwitchScreen(menu, 80);
        }

        LoadMainMenu();
    })
    .Run();
