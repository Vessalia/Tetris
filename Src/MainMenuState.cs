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

        public MainMenuState(IGameStateSwitcher switcher, Input input, Dictionary<string, Song> songs) : base(switcher, input, songs)
        {
            menu = new Menu();

            var playPos = Constants.Screen / 2;

            Action playAction = () =>
            {
                switcher.SetNextState(new PlayState(switcher, input, songs));
            };

            var settingsPos = Constants.Screen / 2 + new Vector2(0, 100);

            Action settingsAction = () =>
            {
                switcher.SetNextState(new SettingsState(switcher, input, songs));
            };

            var exitPos = Constants.Screen / 2 + new Vector2(0, 200);

            Action exitAction = () =>
            {
                switcher.SetNextState(null);
            };

            menu.AddButton(playPos, Color.White, "Play", playAction);
            menu.AddButton(settingsPos, Color.White, "Settings", settingsAction);
            menu.AddButton(exitPos, Color.White, "Exit", exitAction);

            MediaPlayer.Stop();
            MediaPlayer.Volume = 1;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(songs["menu"]);
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
