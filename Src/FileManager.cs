using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Tetris.Src
{
    public class FileManager
    {
        public readonly string highscoresFilename = "Highscores/Highscores.tetris";

        public FileManager()
        {
            string fullpath = Path.GetFullPath(highscoresFilename);

            // Check to see if the save exists
            if (!File.Exists(fullpath))
            {
                //If the file doesn't exist, make a fake one...
                // Create the data to save
                HighscoreData data = new HighscoreData(5);
                data.playerName[0] = "Cooper";
                data.score[0] = 200500;

                data.playerName[1] = "Daniel";
                data.score[1] = 187000;

                data.playerName[2] = "Holly";
                data.score[2] = 113300;

                data.playerName[3] = "A Literal Rock";
                data.score[3] = 95100;

                data.playerName[4] = "Nathan";
                data.score[4] = 1000;

                SaveHighscores(data, highscoresFilename);
            }
        }

        public static HighscoreData LoadHighscores(string filename)
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

        private static void SaveHighscores(HighscoreData data, string filename)
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
    }
}
