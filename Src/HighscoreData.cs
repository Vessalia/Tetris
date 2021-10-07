using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

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
