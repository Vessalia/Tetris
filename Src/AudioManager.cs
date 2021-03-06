using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public class AudioManager
    {
        private readonly Dictionary<string, Song> songs;
        
        private string currSong;

        public int MasterVolume { get; private set; }

        private readonly FileManager<ConfigData> fileManager;

        public AudioManager(ContentManager Content)
        {
            songs = new Dictionary<string, Song>
            {
                ["menu"] = Content.Load<Song>("Tetris (2008) OST - Menu Loop"),
                ["game"] = Content.Load<Song>("Tetris 99 - Main Theme"),
                ["settings"] = Content.Load<Song>("Internet Settings"),
                ["highscores"] = Content.Load<Song>("Outer Wilds Original Soundtrack #03 - The Museum")
            };

            fileManager = new FileManager<ConfigData>(Constants.configPath);

            MasterVolume = fileManager.LoadData().volume;
        }

        public void Update() { }

        public void PlaySong(string song, float volume, bool isRepeating = true)
        {
            MediaPlayer.Stop();
            MediaPlayer.Volume = InternalMasterVolume() * volume;
            MediaPlayer.IsRepeating = isRepeating;
            MediaPlayer.Play(songs[song]);

            currSong = song;
        }

        public void ResumeSong(float volume, bool isRepeating = true)
        {
            MediaPlayer.Volume = InternalMasterVolume() * volume;
            MediaPlayer.IsRepeating = isRepeating;
        }

        public void SetVolume(float volume)
        {
            MediaPlayer.Volume = InternalMasterVolume() * volume;
        }

        public void IncrementVolume(int increment)
        {
            int newVolume = MasterVolume + increment;
            MasterVolume = (int)MathF.Max(MathF.Min(newVolume, 100), 0);
            MediaPlayer.Volume = InternalMasterVolume();

            ConfigData data = fileManager.LoadData();
            ConfigManager configManager = new ConfigManager(data);
            configManager.SetVolume(MasterVolume);
        }

        private float InternalMasterVolume()
        {
            return MasterVolume / 100f;
        }

        public string GetCurrentSong()
        {
            return currSong;
        }
    }
}
