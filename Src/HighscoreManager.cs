using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public class HighscoreManager
    {
        public HighscoreData data { get; private set; }
        public HighscoreManager(HighscoreData data)
        {
            this.data = data;
        }

        public void AddHighscore(string name, int score)
        {
            for (int i = 0; i < data.scores.Count; i++)
            {
                if (data.minScore == data.scores[i] && score > data.minScore)
                {
                    data.scores.RemoveAt(i);
                    data.names.RemoveAt(i);
                }
            }
            data.names.Add(name);
            data.scores.Add(score);
            data.SortData();

            FileManager<HighscoreData> fileManager = new FileManager<HighscoreData>(Constants.highscorePath);
            fileManager.SaveData(data);
        }
    }
}
