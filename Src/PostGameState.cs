using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    class PostGameState : GameState
    {
        private string playerName, tempName = null;
        private int score;

        public PostGameState(IGameStateSwitcher switcher, Input input, AudioManager audioManager, FileManager fileManager, int score) : base(switcher, input, audioManager, fileManager)
        {
            this.score = score;

            audioManager.PlaySong("highscores", 1);
        }

        public override void DrawToScreen(SpriteBatch sb, Dictionary<string, SpriteFont> fonts)
        {
            if (playerName == null)
            {
                DrawInputUI();
            }
            else
            {
                DrawEndScreen();
            }
        }

        public override void HandleInput()
        {
            var keys = input.GetPressedKeys();
            if (keys.Length > 0)
            {
                playerName = UpdateName(keys[0]);
            }
        }

        public override void Update(float timeStep) { }

        private string UpdateName(Keys key)
        {
            if (key == Keys.Enter && tempName != null)
            {
                fileManager.SaveHighScore(score, tempName);
                return tempName;
            }
            else
            {
                tempName += (char)key;
            }

            return null;
        }

        private void DrawInputUI()
        {

        }

        private void DrawEndScreen()
        {

        }
    }
}
