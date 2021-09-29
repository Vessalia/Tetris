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

        public MainMenuState(IGameStateSwitcher switcher, Input input, AudioManager audioManager) : base(switcher, input, audioManager)
        {
            menu = new Menu();

            var buttonSpacing = new Vector2(0, 100);

            var playPos = Constants.Screen / 2 - new Vector2(0, 30);

            Action playAction = () =>
            {
                switcher.SetNextState(new PlayState(switcher, input, audioManager));
            };

            var settingsPos = playPos + buttonSpacing;

            Action settingsAction = () =>
            {
                switcher.SetNextState(new SettingsState(switcher, input, audioManager));
            };

            var highscoresPos = playPos + 2 * buttonSpacing;

            Action highscoresAction = () =>
            {
                switcher.SetNextState(new HighscoreState(switcher, input, audioManager));
            };

            var exitPos = playPos + 3 * buttonSpacing;

            Action exitAction = () =>
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

            sb.DrawString(fonts["title"], text, new Vector2(Constants.Screen.X / 2, 200) - textSize / 2, Color.Red);
        }
    }
}
