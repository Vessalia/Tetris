using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public class ConfigManager
    {
        public ConfigData data { get; private set; }

        private FileManager<ConfigData> fileManager;

        public ConfigManager(ConfigData data)
        {
            this.data = data;

            fileManager = new FileManager<ConfigData>(Constants.configPath);
        }

        public void SaveKeyBinding(string name, Keys key)
        {
            for (int i = 0; i < data.keyNames.Count; i++)
            {
                if (name == data.keyNames[i])
                {
                    data.keyBindings[i] = key;
                }
            }

            fileManager.SaveData(data);
        }

        public Keys GetKeyBinding(string name)
        {
            for (int i = 0; i < data.keyNames.Count; i++)
            {
                if (name.Equals(data.keyNames[i]))
                {
                    return data.keyBindings[i];
                }
            }

            throw new Exception("key binding does not exist, check to see if it has been initialized");
        }

        public void SetVolume(int volume)
        {
            data.volume = volume;

            fileManager.SaveData(data);
        }
    }
}
