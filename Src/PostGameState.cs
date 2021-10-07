using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    class PostGameState : GameState
    {
        private string playerName;
        private int score;

        public PostGameState(IGameStateSwitcher switcher, Input input, AudioManager audioManager, FileManager fileManager, int score) : base(switcher, input, audioManager, fileManager)
        {
            this.score = score;

            playerName = null;

            audioManager.PlaySong("highscores", 1);
        }

        public override void DrawToScreen(SpriteBatch sb, Dictionary<string, SpriteFont> fonts)
        {
            
        }

        public override void HandleInput()
        {
            
        }

        public override void Update(float timeStep)
        {
            
        }
    }
}
