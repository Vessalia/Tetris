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
        private Grid grid;

        private Location pos;
        private Location initialPos;

        private Color colour;

        private bool[,] shape;

        private int xSpeed;

        private bool isLive;

        private float timer;
        private float updateTimer;
        private readonly int fallSpeed;

        private GhostBlock ghostBlock;

        public Block(Location pos, bool[,] shape, Color colour, Grid grid)
        {
            this.pos = pos;
            this.shape = shape;
            this.colour = colour;
            this.grid = grid;

            initialPos = pos;

            isLive = true;

            timer = 0;
            updateTimer = 1;
            fallSpeed = 1;
            ghostBlock = new GhostBlock();
        }

        public void Draw(SpriteBatch sb, int xOffset = 0, int yOffset = 0)
        {
            for (int i = 0; i < shape.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < shape.GetUpperBound(1) + 1; j++)
                {
                    if (shape[j, i])
                    {
                        Vector2 drawPos = Constants.GridToScreenCoords(new Location(pos.x + i + xOffset, pos.y + j + yOffset), grid.GetCellMN());
                        sb.FillRectangle(drawPos, new Size2(grid.GetCellLen(), grid.GetCellLen()), colour);
                        sb.DrawRectangle(drawPos, new Size2(grid.GetCellLen(), grid.GetCellLen()), Color.Black);
                    }
                }
            }
        }

        public void Update(float dt)
        {
            if (!CollisionCheck())
            {
                timer += dt;
                if (timer >= updateTimer)
                {
                    pos.y += fallSpeed;

                    ResetTimers();
                }
            }

            int blockSpawnCheck = 0;
            while (CollisionCheck() || BlockCollisionCheck())
            {
                pos.y -= 1;
                isLive = false;
                blockSpawnCheck++;
                if(blockSpawnCheck > grid.GetCellMN().y)
                {
                    break;
                }
            }
        }

        public bool CollisionCheck()
        {
            for (int i = 0; i < shape.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < shape.GetUpperBound(1) + 1; j++)
                {
                    if (shape[j, i])
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

        public bool BlockCollisionCheck()
        {
            for (int i = 0; i < shape.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < shape.GetUpperBound(1) + 1; j++)
                {
                    if (shape[j, i])
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
            shape = RotateArrayClockwise(shape);

            while (CollisionCheck())
            {
                if (pos.x < 0)
                {
                    pos.x += 1;
                }
                else if (pos.x >= grid.GetCellMN().x - (shape.GetUpperBound(0) + 1))
                {
                    pos.x -= 1;
                }
                else
                {
                    shape = RotateArrayCounterClockwise(shape);
                }
            }
        }

        public void ClampedRotateCounterClockwise(Grid grid)
        {
            shape = RotateArrayCounterClockwise(shape);

            while (CollisionCheck())
            {
                if (pos.x < 0)
                {
                    pos.x += 1;
                }
                else if (pos.x >= grid.GetCellMN().x - (shape.GetUpperBound(0) + 1))
                {
                    pos.x -= 1;
                }
                else
                {
                    shape = RotateArrayClockwise(shape);
                }
            }
        }

        private void ResetTimers()
        {
            timer = 0;
            updateTimer = 1;
        }

        public void HorizontalTranslation(Grid grid, int dir)
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

        public void VerticalTranslation(float clockDiv)
        {
            updateTimer = clockDiv;
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

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public bool IsShapeHere(Location cellPos)
        {
            for (int i = 0; i < shape.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < shape.GetUpperBound(1) + 1; j++)
                {
                    if (shape[j, i])
                    {
                        if (pos + new Location (i, j) == cellPos)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void AddPos(Location dir)
        {
            pos += dir;
        }

        public Location GetPos()
        {
            return pos;
        }

        public Location GetShapeMN()
        {
            return new Location(shape.GetUpperBound(0) + 1, shape.GetUpperBound(1) + 1);
        }

        public void ResetPos()
        {
            pos = initialPos;
        }

        public bool[,] GetShape()
        {
            return shape;
        }

        public Color GetColour()
        {
            return colour;
        }
    }
}
