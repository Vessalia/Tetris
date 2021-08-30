using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris.Src
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Grid grid;

        private GameState gameState;

        private List<Block> blocks;

        private List<Block> placedBlocks;

        private List<Block> onScreenBlocks;

        private Block activeBlock;

        private Controller controller;

        private bool blockRefresh;

        public static Input input;

        Random randInt = new Random();

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

            Block iBlock = ShapeBuilder.CreateIBlock(new Location(grid.GetCellMN().x / 2 - 2, 0));
            Block oBlock = ShapeBuilder.CreateOBlock(new Location(grid.GetCellMN().x / 2 - 2, 0));
            Block tBlock = ShapeBuilder.CreateTBlock(new Location(grid.GetCellMN().x / 2 - 2, 0));
            Block sBlock = ShapeBuilder.CreateSBlock(new Location(grid.GetCellMN().x / 2 - 2, 0));
            Block zBlock = ShapeBuilder.CreateZBlock(new Location(grid.GetCellMN().x / 2 - 2, 0));
            Block jBlock = ShapeBuilder.CreateJBlock(new Location(grid.GetCellMN().x / 2 - 2, 0));
            Block lBlock = ShapeBuilder.CreateLBlock(new Location(grid.GetCellMN().x / 2 - 2, 0));

            blocks = new List<Block>
            {
                iBlock, oBlock, tBlock, sBlock, zBlock, jBlock, lBlock
            };

            placedBlocks = new List<Block>();
            onScreenBlocks = new List<Block>();

            activeBlock = (Block)blocks[randInt.Next(blocks.Count())].Clone();

            onScreenBlocks.Add(activeBlock);

            controller = new Controller(input);
            controller.SetActiveBlock(activeBlock);

            blockRefresh = true;

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

            controller.HandleInput(grid, blockRefresh);

            activeBlock.Update(grid, dt);

            if (!blockRefresh)
            {
                if (input.IsKeyJustReleased(Keys.Down))
                {
                    blockRefresh = true;
                }
            }

            if (!activeBlock.IsBlockLive())
            {
                placedBlocks.Add(activeBlock);
                activeBlock = (Block)blocks[randInt.Next(blocks.Count())].Clone();
                controller.SetActiveBlock(activeBlock);
                onScreenBlocks.Add(activeBlock);
                blockRefresh = false;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Navy);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            grid.DrawGrid(_spriteBatch);

            foreach (var block in onScreenBlocks)
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
