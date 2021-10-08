using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Tetris.Src
{
    public class FileManager
    {
        public readonly string highscoresFilename = "Highscores/Highscores.stor";
        public readonly string keyBindingsFilename = "Settings/KeyBindings.stor";

        public FileManager()
        {
            string scoresFullpath = Path.GetFullPath(highscoresFilename);

            // Check to see if the save exists
            if (!File.Exists(scoresFullpath))
            {
                //If the file doesn't exist, make a fake one...
                // Create the data to save
                HighscoreData data = new HighscoreData(7);
                data.playerName[0] = "Cooper";
                data.score[0] = 960000;

                data.playerName[1] = "Daniel";
                data.score[1] = 55400;

                data.playerName[2] = "Holly";
                data.score[2] = 42500;

                data.playerName[3] = "Boomer";
                data.score[3] = 40300;

                data.playerName[4] = "Buzz";
                data.score[4] = 26500;

                data.playerName[5] = "Moose";
                data.score[5] = 26460;

                data.playerName[6] = "Nathan";
                data.score[6] = -1000;

                SaveHighscores(data, highscoresFilename);
            }

            string keyBindingsFullpath = Path.GetFullPath(keyBindingsFilename);


        }

        public HighscoreData LoadHighscores(string filename)
        {
            HighscoreData data;

            // Get the path of the save game
            string fullpath = Path.GetFullPath(filename);

            // Open the file
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate, FileAccess.Read);
            try
            {
                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(HighscoreData));
                data = (HighscoreData)serializer.Deserialize(stream);
            }
            finally
            {
                // Close the file
                stream.Close();
            }

            return (data);
        }

        private void SaveHighscores(HighscoreData data, string filename)
        {
            // Get the path of the save game
            string fullpath = Path.GetFullPath(filename);

            // Open the file, creating it if necessary
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
            try
            {
                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(HighscoreData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                // Close the file
                stream.Close();
            }
        }

        public void SaveHighScore(int score, string playerName)
        {
            // Create the data to save
            HighscoreData data = LoadHighscores(highscoresFilename);

            int scoreIndex = -1;
            for (int i = 0; i < data.count; i++)
            {
                if (score > data.score[i])
                {
                    scoreIndex = i;
                    break;
                }
            }

            if (scoreIndex > -1)
            {
                //New high score found ... do swaps
                for (int i = data.count - 1; i > scoreIndex; i--)
                {
                    data.playerName[i] = data.playerName[i - 1];
                    data.score[i] = data.score[i - 1];
                }

                data.playerName[scoreIndex] = playerName; //Retrieve User Name Here
                data.score[scoreIndex] = score;

                SaveHighscores(data, highscoresFilename);
            }
        }

        public string GetHighscoreFilePath()
        {
            return highscoresFilename;
        }
    }
}
