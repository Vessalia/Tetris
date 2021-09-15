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

        private GhostBlock ghostBlock;

        private Controller controller;

        private bool blockRefresh;
        private bool heldBlockCooldown;

        private Random randInt;

        private float timer;
        private bool isGameOver;

        private int score;

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

            ghostBlock = new GhostBlock(activeBlock, grid);

            controller = new Controller(input);
            controller.SetActiveBlock(activeBlock);

            blockRefresh = true;
            heldBlockCooldown = false;
            isGameOver = false;

            MediaPlayer.Stop();
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(songs["game"]);

            timer = 4;
            score = 0;
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

                heldBlock.ResetShape();
            }
        }

        public override void Update(float dt)
        {
            if (isGameOver)
            {
                if (timer >= 0)
                {
                    timer -= dt;
                }
                else
                {
                    switcher.SetNextState(new MainMenuState(switcher, input, songs));
                }
                return;
            }

            MediaPlayer.Volume = 1;

            if (input.IsKeyJustPressed(Keys.Escape))
            {
                switcher.SetNextState(new PauseState(switcher, input, this, songs));
            }

            if (!grid.IsClearing())
            {
                activeBlock.Update(dt);
                ghostBlock.Update();
            }

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

                ghostBlock = new GhostBlock(activeBlock, grid);
            }

            if (!activeBlock.IsBlockLive())
            {
                grid.PlaceBlock(activeBlock);

                isGameOver = activeBlock.IsGameOver();

                heldBlockCooldown = false;

                activeBlock = nextBlock;
                nextBlock = (Block)blocks[randInt.Next(blocks.Count())].Clone();

                controller.SetActiveBlock(activeBlock);

                ghostBlock = new GhostBlock(activeBlock, grid);

                if (input.IsKeyDown(Keys.Down))
                {
                    blockRefresh = false;
                }
            }

            grid.CheckLines(dt);
            score = grid.GetScore();
        }

        public override void DrawToScreen(SpriteBatch sb, Dictionary<string, SpriteFont> fonts)
        {
            grid.DrawGrid(sb);

            var nextText = "Next Block";
            var nextTextVec = new Vector2((Constants.Screen.X + grid.GetCellLen() * grid.GetCellMN().x) / 2, 0);
            sb.DrawString(fonts["default"], nextText, nextTextVec, Color.Blue);

            var holdText = "Held Block";
            var holdTextSize = fonts["default"].MeasureString(holdText);
            var holdTextVec = new Vector2((Constants.Screen.X - grid.GetCellLen() * grid.GetCellMN().x) / 2 - holdTextSize.X, 0);
            sb.DrawString(fonts["default"], holdText, holdTextVec, Color.Orange);

            var scoreText = "Score: " + $"{score}";
            var scoreTextSize = fonts["default"].MeasureString(scoreText);
            var scoreTextVec = new Vector2((Constants.Screen.X + grid.GetCellLen() * grid.GetCellMN().x) / 2, Constants.Screen.Y - scoreTextSize.Y);
            sb.DrawString(fonts["default"], scoreText, scoreTextVec, Color.Crimson);

            if (!grid.IsClearing())
            {
                activeBlock.Draw(sb);
                nextBlock.Draw(sb, (grid.GetCellMN() + nextBlock.GetShapeMN()).x / 2, 2);

                ghostBlock.Draw(sb);
            }
            else
            {
                activeBlock.Draw(sb, (grid.GetCellMN() + nextBlock.GetShapeMN()).x / 2, 2);
            }

            if (heldBlock != null)
            {
                heldBlock.Draw(sb, - heldBlock.GetPos().x - heldBlock.GetShapeMN().x, -heldBlock.GetPos().y + 2);
            }

            if (isGameOver)
            {
                var text = "YOU FUCKING LOSE YOU FUCKING LOSER ASS BITCH";
                var font = fonts["default"];
                var textSize = font.MeasureString(text);

                sb.DrawString(font, text, new Vector2(Constants.Screen.X, Constants.Screen.Y) / 2 - textSize / 2, Color.Black);
                return;
            }
        }
    }
}
