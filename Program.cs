using SadConsole.Configuration;
using SadConsole;
using DiceGame;

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

        void SwitchScreen(ScreenObject screen, int width)
        {
            game.ResizeWindow(width, GameSettings.TotalHeight, game.DefaultFont.GetFontSize(IFont.Sizes.One));
            game.Screen = screen;
            screen.IsFocused = true;
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
