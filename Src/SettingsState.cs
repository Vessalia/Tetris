using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    class SettingsState : GameState
    {
        private readonly Menu menu;

        public SettingsState(IGameStateSwitcher switcher, Input input, AudioManager audioManager) : base(switcher, input, audioManager)
        {
            menu = new Menu();

            var volumeIncPos = Constants.Screen / 2 + new Vector2(120, 0);

            Action volumeIncAction = () =>
            {
                audioManager.IncrementVolume(5);
            };

            var volumeDecPos = Constants.Screen / 2 - new Vector2(120, 0);

            Action volumeDecAction = () =>
            {
                audioManager.IncrementVolume(-5);
            };

            var keysPos = Constants.Screen / 2 + new Vector2(0, 100);

            Action keysAction = () =>
            {

            };

            var menuPos = Constants.Screen / 2 + new Vector2(0, 200);

            Action menuAction = () =>
            {
                switcher.SetNextState(new MainMenuState(switcher, input, audioManager));
            };

            menu.AddButton(volumeIncPos, Color.White, "+", volumeIncAction, 50);
            menu.AddButton(volumeDecPos, Color.White, "-", volumeDecAction, 50);
            menu.AddButton(keysPos, Color.White, "Key Bindings", keysAction);
            menu.AddButton(menuPos, Color.White, "Menu", menuAction);

            audioManager.PlaySong("settings", 0.75f);
        }

        public override void DrawToScreen(SpriteBatch sb, Dictionary<string, SpriteFont> fonts)
        {
            menu.DrawButtons(sb, fonts["default"]);

            var titleText = "Settings";
            var titleTextSize = fonts["title"].MeasureString(titleText);

            var volumeText = "Volume";
            var volumeTextSize = fonts["default"].MeasureString(volumeText);

            sb.DrawString(fonts["title"], titleText, new Vector2(Constants.Screen.X / 2, 200) - titleTextSize / 2, Color.IndianRed);
            sb.DrawString(fonts["default"], volumeText, (Constants.Screen - volumeTextSize) / 2 - new Vector2(0, volumeTextSize.Y), Color.IndianRed);
            DrawUiElements(fonts, sb);

        }

        public override void HandleInput()
        {
            menu.HandleButtonInput();
        }

        public override void Update(float timeStep) { }

        private void DrawUiElements(Dictionary <string, SpriteFont> fonts, SpriteBatch sb)
        {
            var volumeText = $"{audioManager.GetMasterVolume()}";
            var volumeTextSize = fonts["default"].MeasureString(volumeText);

            var volumeWidth = 120;
            sb.FillRectangle((Constants.Screen.X - volumeWidth) / 2, (Constants.Screen.Y - volumeTextSize.Y) / 2, volumeWidth, volumeTextSize.Y, Color.White);
            sb.DrawString(fonts["default"], volumeText, (Constants.Screen - volumeTextSize) / 2, Color.Black);
        }
    }
}
