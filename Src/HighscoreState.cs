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
        private readonly HighscoreData data;

        private readonly Menu menu;

        public HighscoreState(IGameStateSwitcher switcher, Input input, AudioManager audioManager) : base(switcher, input, audioManager)
        {
            var fileManager = new FileManager<HighscoreData>(Constants.highscorePath);
            data = fileManager.LoadData();

            menu = new Menu();

            var menuPos = Constants.Screen / 2 + new Vector2(0, Constants.Screen.Y / 2.3f);

            Action<Button> menuAction = (Button button) =>
            {
                switcher.SetNextState(new MainMenuState(switcher, input, audioManager));
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
            DrawUiElements(sb, fonts);
        }

        public override void HandleInput()
        {
            menu.HandleButtonInput();
        }

        public override void Update(float timeStep) { }

        public void DrawUiElements(SpriteBatch sb, Dictionary<string, SpriteFont> fonts)
        {
            var text = "Highscores";
            var textSize = fonts["title"].MeasureString(text);
            sb.DrawString(fonts["title"], text, new Vector2(Constants.Screen.X / 2, Constants.Screen.Y / 7.2f) - textSize / 2, Color.Aqua);

            float posX = Constants.Screen.X / 4;
            float posY = Constants.Screen.Y / 5;
            float spacing = Constants.Screen.Y / 10;
            for (int i = 0; i < data.Names.Count; i++)
            {
                var scoreText = $"{data.Names[i]}" + ": " + $"{data.Scores[i]}";
                var scoreTextSize = fonts["default"].MeasureString(scoreText);

                if (i % 4 == 0 && i != 0)
                {
                    posX += Constants.Screen.X / 2;
                    posY = Constants.Screen.Y / 5;
                }

                posY += spacing;

                sb.DrawString(fonts["default"], scoreText, new Vector2(posX, posY) - scoreTextSize / 2, Color.Aqua);
            }
        }
    }
}
