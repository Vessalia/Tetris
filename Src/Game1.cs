using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tetris.Src
{
    public class Game1 : Game, IGameStateSwitcher
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Dictionary<string, SpriteFont> fonts;

        private AudioManager audioManager;

        private GameState gameState;

        public static Input input;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = (int)Constants.Screen.X;
            _graphics.PreferredBackBufferHeight = (int)Constants.Screen.Y;
            _graphics.ApplyChanges();

            InitializeFiles();

            input = new Input();

            audioManager = new AudioManager(Content);

            gameState = new MainMenuState(this, input, audioManager);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            fonts = new Dictionary<string, SpriteFont>
            {
                ["default"] = Content.Load<SpriteFont>("Arial32"),
                ["title"] = Content.Load<SpriteFont>("TitleFont"),
                ["score"] = Content.Load<SpriteFont>("ScoreFont")
            };
        }

        protected override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            input.Update();

            gameState.HandleInput();

            if (gameState == null)
            {
                Exit();
                return;
            }

            audioManager.Update();

            gameState.Update(dt);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (gameState is PlayState || gameState is HighscoreState || gameState is HighscoreState || gameState is PostGameState) // bad practice
            {
                GraphicsDevice.Clear(Color.DimGray);
            }
            else
            {
                GraphicsDevice.Clear(Color.LightBlue);
            }

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            gameState.DrawToScreen(_spriteBatch, fonts);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetNextState(GameState gameState)
        {
            this.gameState = gameState;
        }

        public void InitializeFiles()
        {
            if (!File.Exists(Path.GetFullPath(Constants.highscorePath)))
            {
                HighscoreData data = new HighscoreData();

                data.Names.Add("Cooper");
                data.Scores.Add(960000);

                data.Names.Add("Daniel");
                data.Scores.Add(55400);

                data.Names.Add("Holly");
                data.Scores.Add(42500);

                data.Names.Add("Boomer");
                data.Scores.Add(40300);

                data.Names.Add("Buzz");
                data.Scores.Add(26500);

                data.Names.Add("Moose");
                data.Scores.Add(26460);

                data.Names.Add("Christopher");
                data.Scores.Add(25040);

                data.Names.Add("Otis");
                data.Scores.Add(19680);

                data.Names.Add("Oscar");
                data.Scores.Add(19640);

                data.Names.Add("Nathan");
                data.Scores.Add(-1000);

                data.SortData();

                string highscoreFullPath = Path.GetFullPath(Constants.highscorePath);
                FileManager<HighscoreData> fileManager = new FileManager<HighscoreData>(highscoreFullPath);
                fileManager.SaveData(data);
            }

            if (!File.Exists(Path.GetFullPath(Constants.configPath)))
            {
                ConfigData confData = new ConfigData();

                confData.volume = 100;

                confData.keyNames.Add("left");
                confData.keyBindings.Add(Keys.Left);

                confData.keyNames.Add("right");
                confData.keyBindings.Add(Keys.Right);

                confData.keyNames.Add("up");
                confData.keyBindings.Add(Keys.Up);

                confData.keyNames.Add("down");
                confData.keyBindings.Add(Keys.Down);

                confData.keyNames.Add("rotate ccw");
                confData.keyBindings.Add(Keys.Z);

                confData.keyNames.Add("rotate cw");
                confData.keyBindings.Add(Keys.X);

                confData.keyNames.Add("hold");
                confData.keyBindings.Add(Keys.C);

                string configFullPath = Path.GetFullPath(Constants.configPath);
                FileManager<ConfigData> confFileManager = new FileManager<ConfigData>(configFullPath);
                confFileManager.SaveData(confData);
            }
        }
    }
}
