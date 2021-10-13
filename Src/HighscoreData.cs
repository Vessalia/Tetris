using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    [Serializable]
    public class HighscoreData
    {
        public List<string> Names { get; set; }
        public List<int> Scores { get; set; }
        public int MinScore { get; set; }
        public int Count
        {
            get { return Names.Count; }
        }

        public HighscoreData()
        {
            Names = new List<string>();
            Scores = new List<int>();
        }

        public void SortData()
        {
            MinScore = int.MaxValue;
            for (int i = 1; i < Scores.Count; i++)
            {
                if (Scores[i - 1] < Scores[i])
                {
                    for (int j = i; j > 0; j--)
                    {
                        int tempScore = Scores[j];
                        Scores[j] = Scores[j - 1];
                        Scores[j - 1] = tempScore;

                        string tempName = Names[j];
                        Names[j] = Names[j - 1];
                        Names[j - 1] = tempName;
                    }
                }
                if (Scores[^1] < MinScore)
                {
                    MinScore = Scores[^1];
                }
            }
        }
    }
}
