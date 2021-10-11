using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    [Serializable]
    public class ConfigData
    {
        public int volume { get; set; }
        public List<string> keyNames { get; set; }
        public List<Keys> keyBindings { get; set; }

        public int Count
        {
            get { return keyNames.Count; }
        }

        public ConfigData()
        {
            volume = 100;
            
            keyNames = new List<string>();
            keyBindings = new List<Keys>();
        }
    }
}
