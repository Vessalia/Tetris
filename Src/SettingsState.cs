using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        private Action<Keys> onKeyInput;

        public SettingsState(IGameStateSwitcher switcher, Input input, AudioManager audioManager) : base(switcher, input, audioManager)
        {
            menu = new Menu();

            var buttonSpacing = new Vector2(0, 75);

            var volumeIncPos = Constants.Screen / 2 + new Vector2(120, -100);

            Action<Button> volumeIncAction = (Button button) =>
            {
                audioManager.IncrementVolume(5);
            };

            var volumeDecPos = Constants.Screen / 2 - new Vector2(120, 100);

            Action<Button> volumeDecAction = (Button button) =>
            {
                audioManager.IncrementVolume(-5);
            };

            var keysPos = Constants.Screen / 2 + new Vector2(0, 50);

            Action<Button> cwKeyAction = (Button button) =>
            {
                button.SetText("Press a Key");
                onKeyInput = (Keys key) =>
                {
                    Constants.keyBindings["rotate cw"] = key;
                    button.SetText("Rotate CW: " + $"{ Constants.keyBindings["rotate cw"]}");
                };
            };

            var menuPos = Constants.Screen / 2 + new Vector2(0, 300);

            Action<Button> menuAction = (Button button) =>
            {
                switcher.SetNextState(new MainMenuState(switcher, input, audioManager));
            };

            menu.AddButton(volumeIncPos, Color.White, "+", volumeIncAction, 50);
            menu.AddButton(volumeDecPos, Color.White, "-", volumeDecAction, 50);
            menu.AddButton(keysPos, Color.White, "Rotate CW: " + $"{Constants.keyBindings["rotate cw"]}", cwKeyAction);
            menu.AddButton(menuPos, Color.White, "Menu", menuAction);

            audioManager.PlaySong("settings", 0.75f);
        }

        public override void DrawToScreen(SpriteBatch sb, Dictionary<string, SpriteFont> fonts)
        {
            menu.DrawButtons(sb, fonts["default"]);
            DrawUiElements(fonts, sb);

        }

        public override void HandleInput()
        {
            menu.HandleButtonInput();
        }

        public override void Update(float timeStep) 
        {
            var keys = input.GetPressedKeys();
            if (keys.Length > 0 && onKeyInput != null)
            {
                onKeyInput.Invoke(keys[0]);
                onKeyInput = null;
            }
        }

        private void DrawUiElements(Dictionary <string, SpriteFont> fonts, SpriteBatch sb)
        {

            var titleText = "Settings";
            var titleTextSize = fonts["title"].MeasureString(titleText);

            var volumeText = "Volume";
            var volumeTextSize = fonts["default"].MeasureString(volumeText);

            sb.DrawString(fonts["title"], titleText, new Vector2(Constants.Screen.X / 2, 100) - titleTextSize / 2, Color.IndianRed);
            sb.DrawString(fonts["default"], volumeText, (Constants.Screen - volumeTextSize) / 2 - new Vector2(0, 100 + volumeTextSize.Y), Color.IndianRed);

            var volumeLevelText = $"{audioManager.GetMasterVolume()}";
            var volumeLevelTextSize = fonts["default"].MeasureString(volumeLevelText);

            var volumeWidth = 120;
            sb.FillRectangle((Constants.Screen.X - volumeWidth) / 2, (Constants.Screen.Y - volumeLevelTextSize.Y) / 2 - 100, volumeWidth, volumeLevelTextSize.Y, Color.White);
            sb.DrawString(fonts["default"], volumeLevelText, (Constants.Screen - volumeLevelTextSize) / 2 - new Vector2(0, 100), Color.Black);

            var keyText = "Key Bindings";
            sb.DrawString(fonts["default"], keyText, (Constants.Screen - fonts["default"].MeasureString(keyText)) / 2 - new Vector2(0, 25), Color.IndianRed);
        }
    }
}
