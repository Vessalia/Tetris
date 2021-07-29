using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public abstract class GameState
    {
        protected IGameStateSwitcher switcher;
        protected Input input;

        public GameState(IGameStateSwitcher switcher, Input input)
        {
            this.switcher = switcher;
            this.input = input;
        }

        public abstract void HandleInput();
        public abstract void Update(float timeStep);
        public abstract void DrawToScreen(SpriteBatch sb, Dictionary<string, SpriteFont> fonts);
    }
}