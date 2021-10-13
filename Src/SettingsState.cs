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

        private readonly List<Keys> forbiddenKeys;

        private readonly ConfigManager configManager;
        private readonly ConfigData data;

        public SettingsState(IGameStateSwitcher switcher, Input input, AudioManager audioManager) : base(switcher, input, audioManager)
        {
            forbiddenKeys = new List<Keys> 
            { 
                Keys.Escape, 
                Keys.Enter, 
                Keys.Space,
                Keys.Back
            };

            var fileManager = new FileManager<ConfigData>(Constants.configPath);
            data = fileManager.LoadData();
            configManager = new ConfigManager(data);

            menu = new Menu();

            var buttonSpacing = new Vector2(0, Constants.Screen.Y * 5 / 48);

            var volumeIncPos = Constants.Screen / 2 + new Vector2(Constants.Screen.X / 10, -Constants.Screen.Y / 7.2f);

            Action<Button> volumeIncAction = (Button button) =>
            {
                audioManager.IncrementVolume(5);
            };

            var volumeDecPos = Constants.Screen / 2 - new Vector2(Constants.Screen.X / 10, Constants.Screen.Y / 7.2f);

            Action<Button> volumeDecAction = (Button button) =>
            {
                audioManager.IncrementVolume(-5);
            };

            var ccwKeyPos = Constants.Screen / 2 + new Vector2(-Constants.Screen.X / 7, Constants.Screen.Y / 28.8f);

            Action<Button> ccwKeyAction = (Button button) =>
            {
                button.SetText("Bind an Unused Key");
                onKeyInput = (Keys key) =>
                {
                    configManager.SaveKeyBinding("rotate ccw", key);
                    button.SetText("Rotate CCW: " + $"{ key }");
                };
            };

            var cwKeyPos = ccwKeyPos + buttonSpacing;

            Action<Button> cwKeyAction = (Button button) =>
            {
                button.SetText("Bind an Unused Key");
                onKeyInput = (Keys key) =>
                {
                    configManager.SaveKeyBinding("rotate cw", key);
                    button.SetText("Rotate CW: " + $"{ key }");
                };
            };

            var holdKeyPos = ccwKeyPos + 2 * buttonSpacing;

            Action<Button> holdKeyAction = (Button button) =>
            {
                button.SetText("Bind an Unused Key");
                onKeyInput = (Keys key) =>
                {
                    configManager.SaveKeyBinding("hold", key);
                    button.SetText("Hold: " + $"{ key }");
                };
            };

            var leftKeyPos = ccwKeyPos + new Vector2(2 * Constants.Screen.X / 7, 0);

            Action<Button> leftKeyAction = (Button button) =>
            {
                button.SetText("Bind an Unused Key");
                onKeyInput = (Keys key) =>
                {
                    configManager.SaveKeyBinding("left", key);
                    button.SetText("Move Left: " + $"{ key }");
                };
            };

            var rightKeyPos = leftKeyPos + buttonSpacing;

            Action<Button> rightKeyAction = (Button button) =>
            {
                button.SetText("Bind an Unused Key");
                onKeyInput = (Keys key) =>
                {
                    configManager.SaveKeyBinding("right", key);
                    button.SetText("Move Right: " + $"{ key }");
                };
            };

            var downKeyPos = leftKeyPos + 2 * buttonSpacing;

            Action<Button> downKeyAction = (Button button) =>
            {
                button.SetText("Bind an Unused Key");
                onKeyInput = (Keys key) =>
                {
                    configManager.SaveKeyBinding("down", key);
                    button.SetText("Move Down: " + $"{ key }");
                };
            };

            var upKeyPos = leftKeyPos + 3 * buttonSpacing;

            Action<Button> upKeyAction = (Button button) =>
            {
                button.SetText("Bind an Unused Key");
                onKeyInput = (Keys key) =>
                {
                    configManager.SaveKeyBinding("up", key);
                    button.SetText("Move Up: " + $"{ key }");
                };
            };

            var menuPos = ccwKeyPos + 4 * buttonSpacing + new Vector2(Constants.Screen.X / 7, 0);

            Action<Button> menuAction = (Button button) =>
            {
                switcher.SetNextState(new MainMenuState(switcher, input, audioManager));
            };

            menu.AddButton(volumeIncPos, Color.White, "+", volumeIncAction, 50);
            menu.AddButton(volumeDecPos, Color.White, "-", volumeDecAction, 50);
            menu.AddButton(ccwKeyPos, Color.White, "Rotate CCW: " + $"{configManager.GetKeyBinding("rotate ccw")}", ccwKeyAction);
            menu.AddButton(cwKeyPos, Color.White, "Rotate CW: " + $"{configManager.GetKeyBinding("rotate cw")}", cwKeyAction);
            menu.AddButton(holdKeyPos, Color.White, "Hold: " + $"{configManager.GetKeyBinding("hold")}", holdKeyAction);
            menu.AddButton(leftKeyPos, Color.White, "Move Left: " + $"{configManager.GetKeyBinding("left")}", leftKeyAction);
            menu.AddButton(rightKeyPos, Color.White, "Move Right: " + $"{configManager.GetKeyBinding("right")}", rightKeyAction);
            menu.AddButton(downKeyPos, Color.White, "Move Down: " + $"{configManager.GetKeyBinding("down")}", downKeyAction);
            menu.AddButton(upKeyPos, Color.White, "Move Up: " + $"{configManager.GetKeyBinding("up")}", upKeyAction);
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
            if (keys.Length > 0 && onKeyInput != null && !data.keyBindings.Contains(keys[0]) && !forbiddenKeys.Contains(keys[0]))
            {
                onKeyInput.Invoke(keys[0]);
                onKeyInput = null;
            }
        }

        private void DrawUiElements(Dictionary <string, SpriteFont> fonts, SpriteBatch sb)
        {

            var titleText = "Settings";
            var titleTextSize = fonts["title"].MeasureString(titleText);

            sb.DrawString(fonts["title"], titleText, new Vector2(Constants.Screen.X / 2, Constants.Screen.Y / 7.2f) - titleTextSize / 2, Color.IndianRed);

            var volumeText = "Volume";
            var volumeTextSize = fonts["default"].MeasureString(volumeText);

            sb.DrawString(fonts["default"], volumeText, (Constants.Screen - volumeTextSize) / 2 - new Vector2(0, Constants.Screen.Y / 7.2f + volumeTextSize.Y), Color.IndianRed);

            var volumeLevelText = $"{audioManager.MasterVolume}";
            var volumeLevelTextSize = fonts["default"].MeasureString(volumeLevelText);

            var volumeWidth = 120;
            sb.FillRectangle((Constants.Screen.X - volumeWidth) / 2, (Constants.Screen.Y - volumeLevelTextSize.Y) / 2 - Constants.Screen.Y / 7.2f, volumeWidth, volumeLevelTextSize.Y, Color.White);
            sb.DrawString(fonts["default"], volumeLevelText, (Constants.Screen - volumeLevelTextSize) / 2 - new Vector2(0, Constants.Screen.Y / 7.2f), Color.Black);

            var keyText = "Key Bindings";
            sb.DrawString(fonts["default"], keyText, (Constants.Screen - fonts["default"].MeasureString(keyText)) / 2 - new Vector2(0, Constants.Screen.Y / 28.8f), Color.IndianRed);
        }
    }
}
