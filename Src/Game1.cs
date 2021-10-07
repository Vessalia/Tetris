using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Tetris.Src
{
    public class Game1 : Game, IGameStateSwitcher
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Dictionary<string, SpriteFont> fonts;

        private AudioManager audioManager;
        private FileManager fileManager;

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

            input = new Input();

            audioManager = new AudioManager(Content);

            fileManager = new FileManager();

            gameState = new MainMenuState(this, input, audioManager, fileManager);

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

            gameState.Update(dt);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (gameState is PlayState || gameState is HighscoreState || gameState is HighscoreState) // bad practice
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
    }
}
