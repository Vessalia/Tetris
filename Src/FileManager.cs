using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Tetris.Src
{
    public class FileManager
    {
        public string highscoresFilename { get; private set; }
        public string keyBindingsFilename { get; private set; }

        public FileManager()
        {
            highscoresFilename = "Highscores/Highscores.stor";
            keyBindingsFilename = "Config/KeyBindings.stor";

            string scoresFullpath = Path.GetFullPath(highscoresFilename);

            if (!File.Exists(scoresFullpath))
            {
                HighscoreData data = new HighscoreData();

                data.AddHighscore("Cooper", 960000);
                data.AddHighscore("Daniel", 55400);
                data.AddHighscore("Holly", 42000);
                data.AddHighscore("Boomer", 40300);
                data.AddHighscore("Buzz", 26500);
                data.highscores.Add("Moose", 26460);
                data.highscores["Nathan"] = -1000;

                SaveData(data, highscoresFilename);
            }

            string keyBindingsFullpath = Path.GetFullPath(keyBindingsFilename);

            if (!File.Exists(keyBindingsFullpath))
            {
                ConfigData confData = new ConfigData();

                confData.volume = 100;

                confData.keyBindings.Add("left", Keys.Left);
                confData.keyBindings.Add("right", Keys.Right);
                confData.keyBindings.Add("down", Keys.Down);
                confData.keyBindings.Add("up", Keys.Up);
                                    
                confData.keyBindings.Add("rotate ccw", Keys.Z);
                confData.keyBindings.Add("rotate cw", Keys.X);
                confData.keyBindings.Add("hold", Keys.C);

                SaveData(confData, keyBindingsFilename);
            }
        }

        public object LoadData(string filename, Type type)
        {
            HighscoreData data;

            string fullpath = Path.GetFullPath(filename);

            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate, FileAccess.Read);
            try
            {
                XmlSerializer serializer = new XmlSerializer(type);
                data = (HighscoreData)serializer.Deserialize(stream);
            }
            finally
            {
                stream.Close();
            }

            return (data);
        }

        private void SaveData(object data, string filename)
        {
            string fullpath = Path.GetFullPath(filename);

            Type objectType = typeof(object);

            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(object));
                serializer.Serialize(stream, data);
            }
            finally
            {
                stream.Close();
            }
        }

        public void SaveHighscore(int score, string playerName)
        {
            HighscoreData data = (HighscoreData)LoadData(highscoresFilename, typeof(HighscoreData));

            bool found = false;
            string highscoreToRemove = "";

            foreach (var name in data.highscores.Keys)
            {
                if (score > data.highscores[name])
                {
                    highscoreToRemove = name;
                    found = true;
                    break;
                }
            }

            if (found)
            {
                if (data.highscores.ContainsKey(highscoreToRemove))
                {
                    data.highscores.Remove(highscoreToRemove);
                    data.highscores.Add(playerName, score);

                    SaveData(data, highscoresFilename);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
