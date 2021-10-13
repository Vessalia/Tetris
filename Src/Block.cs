using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public class Block : ICloneable
    {
        private readonly Grid grid;

        private Location pos;
        private Location initialPos;

        public Color Colour { get; private set; }

        public bool[,] Shape { get; private set; }
        private readonly bool[,] initialShape;

        private int xSpeed;

        private bool isLive;
        private bool placing;

        public bool IsGameOver { get; private set; }
        private bool setTimers;
        private bool startPlacing;

        private float timer;
        private float placementTimer;
        private float updateTimer;
        private float currUpdateTimer;
        private readonly int fallSpeed;

        private readonly Input input;

        private readonly ConfigManager configManager;

        public Block(Location pos, bool[,] shape, Color colour, Grid grid, Input input)
        {
            this.pos = pos;
            this.grid = grid;
            this.input = input;

            initialPos = pos;
            initialShape = Shape;
            Colour = colour;
            Shape = shape;

            isLive = true;
            IsGameOver = false;
            setTimers = true;
            placing = false;
            startPlacing = false;

            timer = 0;
            placementTimer = 0;
            updateTimer = 1;
            currUpdateTimer = updateTimer;
            fallSpeed = 1;

            var fileManager = new FileManager<ConfigData>(Constants.configPath);
            var data = fileManager.LoadData();
            configManager = new ConfigManager(data);
        }

        public void Draw(SpriteBatch sb, int xOffset = 0, int yOffset = 0)
        {
            for (int i = 0; i < Shape.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < Shape.GetUpperBound(1) + 1; j++)
                {
                    if (Shape[j, i])
                    {
                        Vector2 drawPos = Constants.GridToScreenCoords(new Location(pos.x + i + xOffset, pos.y + j + yOffset), grid.cellMN);
                        sb.FillRectangle(drawPos, new Size2(grid.GetCellLen(), grid.GetCellLen()), Colour);
                        sb.DrawRectangle(drawPos, new Size2(grid.GetCellLen(), grid.GetCellLen()), Color.Black);
                    }
                }
            }
        }

        public void Update(float dt)
        {
            if (setTimers)
            {
                ResetTimers();
                setTimers = false;
            }

            if (!CollisionCheck())
            {
                timer += dt;
                if (timer >= updateTimer)
                {
                    pos.y += fallSpeed;

                    ResetTimers();
                }
            }

            while (CollisionCheck() || BlockCollisionCheck())
            {
                pos.y -= 1;
                startPlacing = true;

                if(pos.y + Shape.GetUpperBound(1) + 1 < 0)
                {
                    pos.y = 0;
                    IsGameOver = true;
                    break;
                }
            }

            if (startPlacing)
            {
                WaitingForPlacement(dt);

                if (!placing || input.IsKeyDown(configManager.GetKeyBinding("down")))
                {
                    placing = false;
                    isLive = false;
                    startPlacing = false;
                }
            }
        }

        private bool CollisionCheck()
        {
            for (int i = 0; i < Shape.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < Shape.GetUpperBound(1) + 1; j++)
                {
                    if (Shape[j, i])
                    {
                        if(!grid.InBounds(new Location(pos.x + i, pos.y + j)))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool BlockCollisionCheck()
        {
            for (int i = 0; i < Shape.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < Shape.GetUpperBound(1) + 1; j++)
                {
                    if (Shape[j, i])
                    {
                        Location cellPos = new Location(pos.x + i, pos.y + j);

                        if (!grid.IsCellEmpty(cellPos))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void ClampedRotateClockwise(Grid grid)
        {
            Shape = RotateArrayClockwise(Shape);

            while (CollisionCheck())
            {
                if (pos.x < 0)
                {
                    pos.x += 1;
                }
                else if (pos.x >= grid.cellMN.x - (Shape.GetUpperBound(0) + 1))
                {
                    pos.x -= 1;
                }
                else
                {
                    Shape = RotateArrayCounterClockwise(Shape);
                }
            }
        }

        public void ClampedRotateCounterClockwise(Grid grid)
        {
            Shape = RotateArrayCounterClockwise(Shape);

            while (CollisionCheck())
            {
                if (pos.x < 0)
                {
                    pos.x += 1;
                }
                else if (pos.x >= grid.cellMN.x - (Shape.GetUpperBound(0) + 1))
                {
                    pos.x -= 1;
                }
                else
                {
                    Shape = RotateArrayClockwise(Shape);
                }
            }
        }

        private void ResetTimers()
        {
            timer = 0;
            updateTimer = currUpdateTimer;
        }

        public void HorizontalTranslation(int dir)
        {
            xSpeed = dir;
            pos.x += xSpeed;

            if (CollisionCheck() || BlockCollisionCheck())
            {
                pos.x -= xSpeed;
            }
        }

        public bool IsBlockLive()
        {
            return isLive;
        }

        public bool IsBlockPlacing()
        {
            return placing;
        }

        public void VerticalTranslation(float clockDiv)
        {
            updateTimer = currUpdateTimer * clockDiv;
        }

        private static bool[,] RotateArrayClockwise(bool[,] src)
        {
            int width;
            int height;
            bool[,] dst;

            width = src.GetUpperBound(0) + 1;
            height = src.GetUpperBound(1) + 1;
            dst = new bool[height, width];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    int newRow;
                    int newCol;

                    newRow = col;
                    newCol = height - (row + 1);

                    dst[newCol, newRow] = src[col, row];
                }
            }

            return dst;
        }

        private static bool[,] RotateArrayCounterClockwise(bool[,] src)
        {
            int width;
            int height;
            bool[,] dst;

            width = src.GetUpperBound(0) + 1;
            height = src.GetUpperBound(1) + 1;
            dst = new bool[height, width];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    int newRow;
                    int newCol;

                    newRow = width - (col + 1);
                    newCol = row;

                    dst[newCol, newRow] = src[col, row];
                }
            }

            return dst;
        }

        public void LevelSpeedUp(int level)
        {
            int levelSpeedCap = 15;

            if (level >= levelSpeedCap)
            {
                currUpdateTimer = 1 / 2f;
            }
            currUpdateTimer = 1 - level / (float) levelSpeedCap;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        private float PlaceTime()
        {
            float maxTime = 0.5f;
            var pow = -(grid.GetLevel() - 1) / 5f;
            var term = MathF.Pow(MathF.E, pow);
            return maxTime * (1 - term);
        }

        private void WaitingForPlacement(float timeStep)
        {
            placementTimer += timeStep;

            if (placementTimer > PlaceTime())
            {
                placementTimer = 0;
                placing = false;
            }
            else
            {
                placing = true;
            }
        }

        public Location GetShapeMN()
        {
            return new Location(Shape.GetUpperBound(0) + 1, Shape.GetUpperBound(1) + 1);
        }

        public void ResetPos()
        {
            pos = initialPos;
        }

        public void ResetShape()
        {
            Shape = initialShape;
        }

        public Location GetPos()
        {
            return pos;
        }
    }
}
