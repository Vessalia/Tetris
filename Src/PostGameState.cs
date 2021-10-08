using Microsoft.Xna.Framework;
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
        private string playerName = "";

        private int score;
        private float timer = 0;

        private bool nameEntered = false;
        private bool holdInput = false;

        private Keys prevKey;

        public PostGameState(IGameStateSwitcher switcher, Input input, AudioManager audioManager, FileManager fileManager, int score) : base(switcher, input, audioManager, fileManager)
        {
            this.score = score;

            audioManager.PlaySong("highscores", 1);
        }

        public override void DrawToScreen(SpriteBatch sb, Dictionary<string, SpriteFont> fonts)
        {
            if (!nameEntered)
            {
                if (CheckForNewHighscore(score))
                {
                    DrawInputUI(sb, fonts);
                }
                else
                {
                    nameEntered = true;
                }
            }
            else
            {
                switcher.SetNextState(new HighscoreState(switcher, input, audioManager, fileManager));
            }
        }

        public override void HandleInput()
        {
            var keys = input.GetPressedKeys();
            if (keys.Length > 0)
            {
                UpdateName(keys[0]);
            }
        }

        public override void Update(float timeStep)
        {
            var keys = input.GetPressedKeys();
            bool incrementTimer = false;
            if (keys.Length > 0)
            {
                if (keys[0] == prevKey)
                {
                    incrementTimer = true;
                }
                else
                {
                    incrementTimer = false;
                    timer = 0;
                    prevKey = keys[0];
                }
            }
            else
            {
                incrementTimer = false;
                timer = 0;
                prevKey = Keys.None;
            }
            
            if (incrementTimer && timer < 1)
            {
                timer += timeStep;
            }

            if (timer >= 1)
            {
                holdInput = true;
            }
            else
            {
                holdInput = false;
            }
        }

        private void UpdateName(Keys key)
        {
            if (input.IsKeyJustPressed(key) || holdInput)
            {
                if (key == Keys.Enter && playerName.Length > 0)
                {
                    fileManager.SaveHighScore(score, playerName);
                    nameEntered = true;
                }
                else if (key == Keys.Space && playerName.Length > 0)
                {
                    playerName += " ";
                }
                else if (key == Keys.Back && playerName.Length > 0)
                {
                    playerName = playerName[0..^1];
                }
                else if (input.IsKeyAChar(key))
                {
                    if (input.IsKeyDown(Keys.LeftShift) || input.IsKeyDown(Keys.RightShift))
                    {
                        playerName += key.ToString().ToUpper();
                    }
                    else
                    {
                        playerName += key.ToString().ToLower();
                    }
                }
            }
        }

        private void DrawInputUI(SpriteBatch sb, Dictionary<string, SpriteFont> fonts)
        {
            var highscoreText = "New HighScore!";
            var highscoreTextSize = fonts["title"].MeasureString(highscoreText);
            sb.DrawString(fonts["title"], highscoreText, (new Vector2(Constants.Screen.X, Constants.Screen.Y / 3.6f) - highscoreTextSize) / 2, Color.MediumVioletRed);

            var text = "Type your player name";
            var textSize = fonts["title"].MeasureString(text);
            sb.DrawString(fonts["title"], text, (new Vector2(Constants.Screen.X, Constants.Screen.Y / 3.6f + 2 * highscoreTextSize.Y) - textSize) / 2, Color.MediumVioletRed);

            var playerText = playerName;
            var playerTextSize = fonts["default"].MeasureString(playerText);
            sb.DrawString(fonts["default"], playerText, (Constants.Screen - playerTextSize) / 2, Color.MediumVioletRed);
        }

        private bool CheckForNewHighscore(int score)
        {
            HighscoreData data = fileManager.LoadHighscores(fileManager.GetHighscoreFilePath());

            foreach (var points in data.score)
            {
                if (score > points)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
