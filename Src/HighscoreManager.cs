using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public class HighscoreManager
    {
        public HighscoreData Data { get; private set; }
        public HighscoreManager(HighscoreData data)
        {
            Data = data;
        }

        public void AddHighscore(string name, int score)
        {
            Data.SortData();
            for (int i = 0; i < Data.Scores.Count; i++)
            {
                if (Data.MinScore == Data.Scores[i] && score > Data.MinScore)
                {
                    Data.Scores.RemoveAt(i);
                    Data.Names.RemoveAt(i);
                }
            }
            Data.Names.Add(name);
            Data.Scores.Add(score);
            Data.SortData();

            FileManager<HighscoreData> fileManager = new FileManager<HighscoreData>(Constants.highscorePath);
            fileManager.SaveData(Data);
        }
    }
}
