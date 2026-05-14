using SadConsole.Configuration;
using DiceGame;

Settings.WindowTitle = "Dice Game 2026";
Settings.ResizeMode = Settings.WindowResizeOptions.None;
Settings.AllowWindowResize = false;

Builder
    .GetBuilder()
    .SetWindowSizeInPixels(GameSettings.TotalWidth * 12, GameSettings.TotalHeight * 12)
    .ConfigureFonts((config, game) => 
    {
        game.LoadFont("Assets/Fonts/Cheepicus_12x12.font");
    })
    .OnStart((sender, game) => 
    {
        game.DefaultFont = game.Fonts["Cheepicus12"];
    })
    .SetStartingScreen<DiceGame.Scenes.RootScreen>()
    .IsStartingScreenFocused(true)
    .Run();
