using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    [Serializable]
    public struct ConfigData
    {
        public int volume;

        public Dictionary<string, Keys> keyBindings;

        public int count;

        public ConfigData(int volume = 100)
        {
            this.volume = volume;
            keyBindings = new Dictionary<string, Keys>();

            count = keyBindings.Count;
        }
    }
}
