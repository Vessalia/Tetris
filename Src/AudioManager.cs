using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public class AudioManager
    {
        private Dictionary<string, Song> songs;

        private int masterVolume = 100;

        public AudioManager(ContentManager Content)
        {
            songs = new Dictionary<string, Song>
            {
                ["menu"] = Content.Load<Song>("Tetris (2008) OST - Menu Loop"),
                ["game"] = Content.Load<Song>("Tetris 99 - Main Theme"),
                ["settings"] = Content.Load<Song>("Internet Settings")
            };
        }

        public void PlaySong(string song, float volume, bool isRepeating = true)
        {
            MediaPlayer.Stop();
            MediaPlayer.Volume = InternalMasterVolume() * volume;
            MediaPlayer.IsRepeating = isRepeating;
            MediaPlayer.Play(songs[song]);
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
            int newVolume = masterVolume + increment;
            masterVolume = (int)MathF.Max(MathF.Min(newVolume, 100), 0);
            MediaPlayer.Volume = InternalMasterVolume();
        }

        public int GetMasterVolume()
        {
            return masterVolume;
        }

        private float InternalMasterVolume()
        {
            return masterVolume / 100f;
        }
    }
}
