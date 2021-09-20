using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    class PostGameState : GameState
    {
        public PostGameState(IGameStateSwitcher switcher, Input input, Dictionary<string, Song> songs) : base(switcher, input, songs)
        {

        }

        public override void DrawToScreen(SpriteBatch sb, Dictionary<string, SpriteFont> fonts)
        {
            throw new NotImplementedException();
        }

        public override void HandleInput()
        {
            throw new NotImplementedException();
        }

        public override void Update(float timeStep)
        {
            throw new NotImplementedException();
        }
    }
}
