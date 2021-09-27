using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
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

            var audioPos = Constants.Screen / 2;

            Action audioAction = () =>
            {
                
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

            menu.AddButton(audioPos, Color.White, "Volume", audioAction);
            menu.AddButton(keysPos, Color.White, "Key Bindings", keysAction);
            menu.AddButton(menuPos, Color.White, "Menu", menuAction);

            audioManager.PlaySong("settings", 0.75f);
        }

        public override void DrawToScreen(SpriteBatch sb, Dictionary<string, SpriteFont> fonts)
        {
            menu.DrawButtons(sb, fonts["default"]);

            var text = "Settings";
            var textSize = fonts["default"].MeasureString(text);

            sb.DrawString(fonts["default"], text, new Vector2(Constants.Screen.X / 2, 200) - textSize / 2, Color.IndianRed);
        }

        public override void HandleInput()
        {
            menu.HandleButtonInput();
        }

        public override void Update(float timeStep) { }
    }
}
