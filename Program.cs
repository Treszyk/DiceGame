using SadConsole.Configuration;
using DiceGame;

Settings.WindowTitle = "Dice Game 2026";
Settings.ResizeMode = Settings.WindowResizeOptions.None;

Builder
    .GetBuilder()
    .SetWindowSizeInCells(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
    .ConfigureFonts((config, game) => 
    {
        game.LoadFont("Assets/Fonts/Cheepicus_12x12.font");
    })
    .SetStartingScreen<DiceGame.Scenes.RootScreen>()
    .IsStartingScreenFocused(true)
    .Run();
