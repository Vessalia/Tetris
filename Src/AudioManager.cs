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

        private float masterVolume = 1;

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
            MediaPlayer.Volume = masterVolume * volume;
            MediaPlayer.IsRepeating = isRepeating;
            MediaPlayer.Play(songs[song]);
        }

        public void ResumeSong(float volume, bool isRepeating = true)
        {
            MediaPlayer.Volume = masterVolume * volume;
            MediaPlayer.IsRepeating = isRepeating;
        }

        public void SetVolume(float volume)
        {
            MediaPlayer.Volume = masterVolume * volume;
        }
    }
}
