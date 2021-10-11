using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    [Serializable]
    public class HighscoreData
    {
        public List<string> names { get; set; }
        public List<int> scores { get; set; }
        public int minScore { get; set; }
        public int Count
        {
            get { return names.Count; }
        }

        public HighscoreData()
        {
            names = new List<string>();
            scores = new List<int>();
        }

        public void SortData()
        {
            minScore = int.MaxValue;
            for (int i = 1; i < scores.Count; i++)
            {
                if (scores[i - 1] > scores[i])
                {
                    for (int j = i; j > 0; j--)
                    {
                        int tempScore = scores[j];
                        scores[j] = scores[j - 1];
                        scores[j - 1] = tempScore;

                        string tempName = names[j];
                        names[j] = names[j - 1];
                        names[j - 1] = tempName;
                    }
                }
                if (scores[0] < minScore)
                {
                    minScore = scores[0];
                }
            }
        }
    }
}
