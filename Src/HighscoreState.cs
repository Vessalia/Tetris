using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Tetris.Src
{
    class HighscoreState : GameState
    {
        public HighscoreState(IGameStateSwitcher switcher, Input input, AudioManager audioManager, FileManager fileManager) : base(switcher, input, audioManager, fileManager)
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

        public override void Update(float timeStep) { }
    }
}
