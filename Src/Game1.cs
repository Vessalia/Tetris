using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System.Collections.Generic;

namespace Tetris.Src
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Grid grid;

        private GameState gameState;

        private List<Block> blocks;

        public static Input input;

        private float dt;

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

            grid = new Grid(new Location(10, 20));

            Block iBlock = ShapeBuilder.CreateIBlock(new Location(0, 0));
            Block oBlock = ShapeBuilder.CreateOBlock(new Location(4, 0));
            Block tBlock = ShapeBuilder.CreateTBlock(new Location(0, 4));
            Block sBlock = ShapeBuilder.CreateSBlock(new Location(4, 4));
            Block zBlock = ShapeBuilder.CreateZBlock(new Location(0, 8));
            Block jBlock = ShapeBuilder.CreateJBlock(new Location(4, 8));
            Block lBlock = ShapeBuilder.CreateLBlock(new Location(0, 12));

            blocks = new List<Block>
            { 
                iBlock,
                oBlock,
                tBlock,
                sBlock, 
                zBlock,
                jBlock,
                lBlock
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            input.Update();

            foreach (var block in blocks)
            {
                block.Update(grid, dt);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Navy);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            grid.DrawGrid(_spriteBatch);

            foreach (var block in blocks)
            {
                block.Draw(grid, _spriteBatch);
            }

            _spriteBatch.DrawLine(Constants.Screen.X / 2, 0, Constants.Screen.X / 2, Constants.Screen.Y, Color.Orange);
            _spriteBatch.DrawLine(0, Constants.Screen.Y / 2, Constants.Screen.X, Constants.Screen.Y / 2, Color.Orange);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetNextState(GameState gameState)
        {
            this.gameState = gameState;
        }
    }
}
