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
    class PauseState : GameState
    {
        private readonly Menu menu;

        private readonly GameState gameState;

        public PauseState(IGameStateSwitcher switcher, Input input, GameState gameState, AudioManager audioManager) : base(switcher, input, audioManager)
        {
            menu = new Menu();

            this.gameState = gameState;

            var resumePos = Constants.Screen / 2;

            Action resumeAction = () =>
            {
                switcher.SetNextState(gameState);
                audioManager.SetVolume(1);
            };

            var menuPos = Constants.Screen / 2 + new Vector2(0, 100);

            Action menuAction = () =>
            {
                switcher.SetNextState(new MainMenuState(switcher, input, audioManager));
                audioManager.SetVolume(1);
            };

            var exitPos = Constants.Screen / 2 + new Vector2(0, 200);

            Action exitAction = () =>
            {
                switcher.SetNextState(null);
            };

            menu.AddButton(resumePos, Color.White, "Resume", resumeAction);
            menu.AddButton(menuPos, Color.White, "Menu", menuAction);
            menu.AddButton(exitPos, Color.White, "Exit", exitAction);

            audioManager.SetVolume(0.2f);
        }

        public override void HandleInput()
        {
            menu.HandleButtonInput();
        }

        public override void Update(float timeStep) 
        {
            if (input.IsKeyJustPressed(Keys.Escape))
            {
                switcher.SetNextState(gameState);
            }
        }

        public override void DrawToScreen(SpriteBatch sb, Dictionary<string, SpriteFont> fonts)
        {
            var font = fonts["default"];

            gameState.DrawToScreen(sb, fonts);

            sb.FillRectangle(new Vector2(0, 0), Constants.Screen, new Color(Color.DimGray, 0.84f));

            menu.DrawButtons(sb, font);

            var text = "Paused";
            var textSize = font.MeasureString(text);

            sb.DrawString(font, text, new Vector2(Constants.Screen.X / 2, 200) - textSize / 2, Color.Purple);
        }
    }
}
