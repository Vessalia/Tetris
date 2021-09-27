using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public abstract class GameState
    {
        protected IGameStateSwitcher switcher;
        protected Input input;
        protected AudioManager audioManager;

        public GameState(IGameStateSwitcher switcher, Input input, AudioManager audioManager)
        {
            this.switcher = switcher;
            this.input = input;
            this.audioManager = audioManager;
        }

        public abstract void HandleInput();
        public abstract void Update(float timeStep);
        public abstract void DrawToScreen(SpriteBatch sb, Dictionary<string, SpriteFont> fonts);
    }
}