using Microsoft.Xna.Framework;
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

        private Block activeBlock, nextBlock, heldBlock, tempBlock;

        private Controller controller;

        private bool blockRefresh;
        private bool heldBlockCooldown;

        private Random randInt;

        public PlayState(IGameStateSwitcher switcher, Input input, Dictionary<string, Song> songs) : base(switcher, input, songs)
        {
            randInt = new Random();

            grid = new Grid(new Location(10, 20));

            Block iBlock = ShapeBuilder.CreateIBlock(new Location(grid.GetCellMN().x / 2 - 2, 0), grid);
            Block oBlock = ShapeBuilder.CreateOBlock(new Location(grid.GetCellMN().x / 2 - 2, 0), grid);
            Block tBlock = ShapeBuilder.CreateTBlock(new Location(grid.GetCellMN().x / 2 - 2, 0), grid);
            Block sBlock = ShapeBuilder.CreateSBlock(new Location(grid.GetCellMN().x / 2 - 2, 0), grid);
            Block zBlock = ShapeBuilder.CreateZBlock(new Location(grid.GetCellMN().x / 2 - 2, 0), grid);
            Block jBlock = ShapeBuilder.CreateJBlock(new Location(grid.GetCellMN().x / 2 - 2, 0), grid);
            Block lBlock = ShapeBuilder.CreateLBlock(new Location(grid.GetCellMN().x / 2 - 2, 0), grid);

            blocks = new List<Block>
            {
                iBlock, oBlock, tBlock, sBlock, zBlock, jBlock, lBlock
            };

            activeBlock = (Block)blocks[randInt.Next(blocks.Count())].Clone();
            nextBlock = (Block)blocks[randInt.Next(blocks.Count())].Clone();

            controller = new Controller(input);
            controller.SetActiveBlock(activeBlock);

            blockRefresh = true;
            heldBlockCooldown = false;

            MediaPlayer.Stop();
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(songs["game"]);
        }

        public override void HandleInput()
        {
            controller.HandleInput(grid, blockRefresh);
            if (input.IsKeyJustPressed(Keys.C) && !heldBlockCooldown)
            {
                if (heldBlock != null)
                {
                    tempBlock = heldBlock;
                }

                activeBlock.ResetPos();
                heldBlock = activeBlock;
            }
        }

        public override void Update(float dt)
        {
            MediaPlayer.Volume = 1;

            if (input.IsKeyJustPressed(Keys.Escape))
            {
                switcher.SetNextState(new PauseState(switcher, input, this, songs));
            }

            activeBlock.Update(dt);

            if (!blockRefresh)
            {
                if (input.IsKeyJustReleased(Keys.Down) || input.IsKeyJustReleased(Keys.C))
                {
                    blockRefresh = true;
                }
            }

            if (heldBlock == activeBlock && !heldBlockCooldown)
            {
                heldBlockCooldown = true;
                if (tempBlock == null)
                {
                    activeBlock = nextBlock;
                    nextBlock = (Block)blocks[randInt.Next(blocks.Count())].Clone();
                }
                else
                {
                    activeBlock = tempBlock;
                }
                controller.SetActiveBlock(activeBlock);
                {
                    blockRefresh = false;
                }
            }

            if (!activeBlock.IsBlockLive())
            {
                grid.PlaceBlock(activeBlock);
                heldBlockCooldown = false;
                activeBlock = nextBlock;
                nextBlock = (Block)blocks[randInt.Next(blocks.Count())].Clone();
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

            activeBlock.Draw(sb);

            var nextText = "Next Block";
            var nextTextSize = fonts["default"].MeasureString(nextText);
            var nextTextVec = new Vector2((Constants.Screen.X + grid.GetCellLen() * grid.GetCellMN().x) / 2, 0);
            sb.DrawString(fonts["default"], nextText, nextTextVec, Color.Blue);

            var holdText = "Held Block";
            var holdTextSize = fonts["default"].MeasureString(holdText);
            var holdTextVec = new Vector2((Constants.Screen.X - grid.GetCellLen() * grid.GetCellMN().x) / 2 - holdTextSize.X, 0);
            sb.DrawString(fonts["default"], holdText, holdTextVec, Color.Orange);

            activeBlock.Draw(sb);
            nextBlock.Draw(sb, (grid.GetCellMN() + nextBlock.GetShapeMN()).x / 2, 2);

            if (heldBlock != null)
            {
                heldBlock.Draw(sb, - heldBlock.GetPos().x - heldBlock.GetShapeMN().x, -heldBlock.GetPos().y + 2);
            }
        }
    }
}
