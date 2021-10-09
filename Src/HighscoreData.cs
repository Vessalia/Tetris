using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    [Serializable]
    public struct HighscoreData
    {
        public Dictionary<string, int> highscores;

        public int count;

        public HighscoreData(int i = 0)
        {
            highscores = new Dictionary<string, int>();

            count = highscores.Count;
        }

        public void AddHighscore(string name, int score)
        {
            highscores.Add(name, score);
        }
    }
}
