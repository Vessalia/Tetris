using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    class HighscoreState : GameState
    {
        private HighscoreData highscoreData;

        private Menu menu;

        public HighscoreState(IGameStateSwitcher switcher, Input input, AudioManager audioManager, FileManager fileManager) : base(switcher, input, audioManager, fileManager)
        {
            menu = new Menu();

            var menuPos = Constants.Screen / 2 + new Vector2(0, Constants.Screen.Y / 2.3f);

            Action<Button> menuAction = (Button button) =>
            {
                switcher.SetNextState(new MainMenuState(switcher, input, audioManager, fileManager));
            };

            menu.AddButton(menuPos, Color.White, "Menu", menuAction);

            if (audioManager.GetCurrentSong() != "highscores")
            {
                audioManager.PlaySong("highscores", 1);
            }
        }

        public override void DrawToScreen(SpriteBatch sb, Dictionary<string, SpriteFont> fonts)
        {
            menu.DrawButtons(sb, fonts["default"]);
        }

        public override void HandleInput()
        {
            menu.HandleButtonInput();
        }

        public override void Update(float timeStep) { }
    }
}
