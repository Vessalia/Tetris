using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public class MainMenuState : GameState
    {
        private readonly Menu menu;

        public MainMenuState(IGameStateSwitcher switcher, Input input, AudioManager audioManager, FileManager fileManager) : base(switcher, input, audioManager, fileManager)
        {
            menu = new Menu();

            var buttonSpacing = new Vector2(0, Constants.Screen.Y / 7.2f);

            var playPos = Constants.Screen / 2 - new Vector2(0, Constants.Screen.Y / 24f);

            Action<Button> playAction = (Button button) =>
            {
                switcher.SetNextState(new PlayState(switcher, input, audioManager, fileManager));
            };

            var settingsPos = playPos + buttonSpacing;

            Action<Button> settingsAction = (Button button) =>
            {
                switcher.SetNextState(new SettingsState(switcher, input, audioManager, fileManager));
            };

            var highscoresPos = playPos + 2 * buttonSpacing;

            Action<Button> highscoresAction = (Button button) =>
            {
                switcher.SetNextState(new HighscoreState(switcher, input, audioManager, fileManager));
            };

            var exitPos = playPos + 3 * buttonSpacing;

            Action<Button> exitAction = (Button button) =>
            {
                switcher.SetNextState(null);
            };

            menu.AddButton(playPos, Color.White, "Play", playAction);
            menu.AddButton(settingsPos, Color.White, "Settings", settingsAction);
            menu.AddButton(highscoresPos, Color.White, "Highscores", highscoresAction);
            menu.AddButton(exitPos, Color.White, "Exit", exitAction);

            audioManager.PlaySong("menu", 1);
        }

        public override void HandleInput()
        {
            menu.HandleButtonInput();
        }

        public override void Update(float timeStep) { }

        public override void DrawToScreen(SpriteBatch sb, Dictionary<string, SpriteFont> fonts)
        {
            menu.DrawButtons(sb, fonts["default"]);

            var text = "Tetris";
            var textSize = fonts["title"].MeasureString(text);

            sb.DrawString(fonts["title"], text, new Vector2(Constants.Screen.X / 2, 100) - textSize / 2, Color.Red);
        }
    }
}
