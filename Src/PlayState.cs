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
        private readonly Grid grid;

        private readonly List<Block> blocks;

        private Block activeBlock, nextBlock, heldBlock, tempBlock;

        private GhostBlock ghostBlock;

        private readonly Controller controller;

        private bool blockRefresh;
        private bool heldBlockCooldown;

        private readonly Random randInt;

        private float timer;
        private bool isGameOver;

        private int score;
        private int level;

        private readonly ConfigManager configManager;
        private readonly ConfigData data;

        public PlayState(IGameStateSwitcher switcher, Input input, AudioManager audioManager) : base(switcher, input, audioManager)
        {
            FileManager<ConfigData> fileManager = new FileManager<ConfigData>(Constants.configPath);
            data = fileManager.LoadData();
            configManager = new ConfigManager(data);

            randInt = new Random();

            grid = new Grid(new Location(10, 20));

            Block iBlock = ShapeBuilder.CreateIBlock(new Location(grid.CellMN.x / 2 - 2, 0), grid, input);
            Block oBlock = ShapeBuilder.CreateOBlock(new Location(grid.CellMN.x / 2 - 2, 0), grid, input);
            Block tBlock = ShapeBuilder.CreateTBlock(new Location(grid.CellMN.x / 2 - 2, 0), grid, input);
            Block sBlock = ShapeBuilder.CreateSBlock(new Location(grid.CellMN.x / 2 - 2, 0), grid, input);
            Block zBlock = ShapeBuilder.CreateZBlock(new Location(grid.CellMN.x / 2 - 2, 0), grid, input);
            Block jBlock = ShapeBuilder.CreateJBlock(new Location(grid.CellMN.x / 2 - 2, 0), grid, input);
            Block lBlock = ShapeBuilder.CreateLBlock(new Location(grid.CellMN.x / 2 - 2, 0), grid, input);

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

            audioManager.PlaySong("game", 1);

            timer = 4;
            score = 0;
        }

        public override void HandleInput()
        {
            controller.HandleInput(grid, blockRefresh);
            if (input.IsKeyJustPressed(configManager.GetKeyBinding("hold")) && !heldBlockCooldown)
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
                    switcher.SetNextState(new PostGameState(switcher, input, audioManager, score));
                }
                return;
            }

            if (input.IsKeyJustPressed(Keys.Escape))
            {
                switcher.SetNextState(new PauseState(switcher, input, this, audioManager));
            }

            activeBlock.LevelSpeedUp(level);

            if (!grid.Clearing)
            {
                activeBlock.Update(dt);
                ghostBlock.Update();
            }

            if (!blockRefresh)
            {
                if (input.IsKeyJustReleased(configManager.GetKeyBinding("down")) || input.IsKeyJustReleased(configManager.GetKeyBinding("hold")))
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

            if (!activeBlock.IsBlockLive() && !activeBlock.IsBlockPlacing())
            {
                grid.PlaceBlock(activeBlock);

                isGameOver = activeBlock.IsGameOver;

                if (!isGameOver)
                {
                    heldBlockCooldown = false;

                    activeBlock = nextBlock;
                    nextBlock = (Block)blocks[randInt.Next(blocks.Count())].Clone();

                    controller.SetActiveBlock(activeBlock);

                    ghostBlock = new GhostBlock(activeBlock, grid);

                    if (input.IsKeyDown(configManager.GetKeyBinding("down")))
                    {
                        blockRefresh = false;
                    }
                }
            }

            grid.CheckLines(dt);
            score = grid.Score;
            level = grid.GetLevel();
        }

        public override void DrawToScreen(SpriteBatch sb, Dictionary<string, SpriteFont> fonts)
        {
            grid.DrawGrid(sb);

            var nextText = "Next Block";
            var nextTextVec = new Vector2((Constants.Screen.X + grid.GetCellLen() * grid.CellMN.x) / 2, 0);
            sb.DrawString(fonts["default"], nextText, nextTextVec, Color.Blue);

            var holdText = "Held Block";
            var holdTextSize = fonts["default"].MeasureString(holdText);
            var holdTextVec = new Vector2((Constants.Screen.X - grid.GetCellLen() * grid.CellMN.x) / 2 - holdTextSize.X, 0);
            sb.DrawString(fonts["default"], holdText, holdTextVec, Color.Orange);

            var scoreText = "Score: " + $"{score}";
            var scoreTextSize = fonts["default"].MeasureString(scoreText);
            var scoreTextVec = new Vector2((Constants.Screen.X + grid.GetCellLen() * grid.CellMN.x) / 2, Constants.Screen.Y - scoreTextSize.Y);
            sb.DrawString(fonts["default"], scoreText, scoreTextVec, Color.Crimson);

            var levelText = "Level: " + $"{level}";
            var levelTextSize = fonts["default"].MeasureString(scoreText);
            var levelTextVec = new Vector2((Constants.Screen.X + grid.GetCellLen() * grid.CellMN.x) / 2, Constants.Screen.Y - scoreTextSize.Y - levelTextSize.Y);
            sb.DrawString(fonts["default"], levelText, levelTextVec, Color.MistyRose);

            if (isGameOver)
            {
                var text = "YOU FUCKING LOSE YOU FUCKING LOSER ASS BITCH";
                var font = fonts["default"];
                var textSize = font.MeasureString(text);

                sb.DrawString(font, text, new Vector2(Constants.Screen.X, Constants.Screen.Y) / 2 - textSize / 2, Color.Black);
                return;
            }

            if (!grid.Clearing)
            {
                activeBlock.Draw(sb);
                nextBlock.Draw(sb, (grid.CellMN + nextBlock.GetShapeMN()).x / 2, 2);

                ghostBlock.Draw(sb);
            }
            else
            {
                activeBlock.Draw(sb, (grid.CellMN + nextBlock.GetShapeMN()).x / 2, 2);
            }

            if (heldBlock != null)
            {
                heldBlock.Draw(sb, - heldBlock.GetPos().x - heldBlock.GetShapeMN().x, -heldBlock.GetPos().y + 2);
            }
        }
    }
}
