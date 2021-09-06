using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tetris.Src
{
    class PlayState : GameState
    {
        private Grid grid;

        private List<Block> blocks;

        private List<Block> placedBlocks;

        private Block activeBlock;

        private Controller controller;

        private bool blockRefresh;

        private Random randInt;

        public PlayState(IGameStateSwitcher switcher, Input input, Dictionary<string, Song> songs) : base(switcher, input, songs)
        {
            randInt = new Random();

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

            activeBlock = (Block)blocks[randInt.Next(blocks.Count())].Clone();

            controller = new Controller(input);
            controller.SetActiveBlock(activeBlock);

            blockRefresh = true;

            MediaPlayer.Stop();
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(songs["game"]);
        }

        public override void HandleInput()
        {
            controller.HandleInput(grid, blockRefresh);
        }


        public override void Update(float dt)
        {
            MediaPlayer.Volume = 1;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                switcher.SetNextState(new PauseState(switcher, input, this, songs));
            }

            activeBlock.Update(grid, dt, placedBlocks);

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
                if (input.IsKeyDown(Keys.Down))
                {
                    blockRefresh = false;
                }
            }

            grid.CheckLines();
        }

        public override void DrawToScreen(SpriteBatch sb, Dictionary<string, SpriteFont> fonts)
        {
            grid.DrawGrid(sb);

            foreach (var block in placedBlocks)
            {
                block.Draw(grid, sb);
            }

            activeBlock.Draw(grid, sb);
        }
    }
}
