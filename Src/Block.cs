using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    class Block
    {
        private Location pos;

        private bool[,] shape;

        private int xSpeed;

        private bool isLive;

        private float timer;
        private float updateTimer;
        private int fallSpeed;

        public Block(Location pos, bool[,] shape)
        {
            this.pos = pos;
            this.shape = shape;

            isLive = true;

            timer = 0;
            updateTimer = 1;
            fallSpeed = 1;
        }

        public void Draw(Grid grid, SpriteBatch sb)
        {
            Vector2 drawPos = Vector2.Zero;
            for (int i = 0; i < shape.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < shape.GetUpperBound(1) + 1; j++)
                {
                    if (shape[j, i])
                    {
                        drawPos = Constants.GridToScreenCoords(new Location(pos.x + i, pos.y + j), grid.GetCellMN());
                        sb.FillRectangle(drawPos, new Size2(grid.GetCellLen(), grid.GetCellLen()), Color.White);
                        sb.DrawRectangle(drawPos, new Size2(grid.GetCellLen(), grid.GetCellLen()), Color.Black);
                    }
                }
            }
        }

        public void Update(Grid grid, float dt)
        {
            if (isLive)
            {
                if (CollisionCheck(grid))
                {
                    timer += dt;
                    if (timer >= updateTimer)
                    {
                        pos.y += fallSpeed;

                        ResetTimers();
                    }
                }

                if (!CollisionCheck(grid))
                {
                    pos.y -= 1;
                    fallSpeed = 0;
                    isLive = false;
                    ResetTimers();
                }
            }
        }

        public bool CollisionCheck(Grid grid)
        {
            for (int i = 0; i < shape.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < shape.GetUpperBound(1) + 1; j++)
                {
                    if (shape[j, i])
                    {
                        if(!grid.InBounds(new Location(pos.x + i, pos.y + j)))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void ResetTimers()
        {
            timer = 0;
            updateTimer = 1;
        }

        public void Rotate(Grid grid)
        {
            shape = RotateArrayClockwise(shape);
        }

        public void HorizontalTranslation(Grid grid, int dir)
        {
            xSpeed = dir;
            pos.x += xSpeed;
            if (!CollisionCheck(grid))
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

        private bool BlockHasReachcedBottom()
        {
            return true;
        }
    }
}
