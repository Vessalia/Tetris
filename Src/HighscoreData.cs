using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    [Serializable]
    public struct HighscoreData
    {
        public string[] playerName;
        public int[] score;

        public int count;
        public HighscoreData(int count)
        {
            playerName = new string[count];
            score = new int[count];

            this.count = count;
        }
    }
}
