using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public class ConfigManager
    {
        public ConfigData Data { get; private set; }

        private readonly FileManager<ConfigData> fileManager;

        public ConfigManager(ConfigData data)
        {
            this.Data = data;

            fileManager = new FileManager<ConfigData>(Constants.configPath);
        }

        public void SaveKeyBinding(string name, Keys key)
        {
            for (int i = 0; i < Data.keyNames.Count; i++)
            {
                if (name == Data.keyNames[i])
                {
                    Data.keyBindings[i] = key;
                }
            }

            fileManager.SaveData(Data);
        }

        public Keys GetKeyBinding(string name)
        {
            for (int i = 0; i < Data.keyNames.Count; i++)
            {
                if (name.Equals(Data.keyNames[i]))
                {
                    return Data.keyBindings[i];
                }
            }

            throw new Exception("key binding does not exist, check to see if it has been initialized");
        }

        public void SetVolume(int volume)
        {
            Data.volume = volume;

            fileManager.SaveData(Data);
        }
    }
}
