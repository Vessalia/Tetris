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

        private float timer;
        private float updateTimer;
        private int fallSpeed;

        private Grid grid;

        public Block(Location pos, bool[,] shape)
        {
            this.pos = pos;
            this.shape = shape;

            timer = 0;
            updateTimer = 1;
            fallSpeed = 1;
        }

        public void Draw(Grid grid, SpriteBatch sb)
        {
            Vector2 drawPos = Vector2.Zero;
            for (int i = 0; i < shape.GetLength(1); i++)
            {
                for (int j = 0; j < shape.GetLength(0); j++)
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
            timer += dt;
            if(CollisionCheck(grid, 0))
            {
                if (timer >= updateTimer)
                {
                    pos.y += fallSpeed;

                    ResetTimers();
                }
            }
        }

        public bool CollisionCheck(Grid grid, int dir)
        {
            var shapeNeighbors = new List<List<Location>>();
            int minX = 4;
            int maxX = 0;
            int minY = 4;
            int maxY = 0;
            for (int i = 0; i < shape.GetLength(1); i++)
            {
                for (int j = 0; j < shape.GetLength(0); j++)
                {
                    if (shape[j, i])
                    {
                        minX = (int)MathF.Min(minX, i); maxX = (int)MathF.Max(maxX, i);
                        minY = (int)MathF.Min(minY, j); maxY = (int)MathF.Max(maxY, j);
                        shapeNeighbors.Add(grid.Neighbors(new Location(pos.x + i, pos.y + j)));
                    }
                }
            }

            foreach (var cellNeighbors in shapeNeighbors)
            {
                foreach (var neighbor in cellNeighbors)
                {
                    bool rightCheck = dir > 0 && !grid.InBounds(pos + new Location(maxX + dir, 0));
                    bool leftCheck = dir < 0 && !grid.InBounds(pos + new Location(minX + dir, 0));
                    bool bottomCheck = dir == 0 && !grid.InBounds(pos + new Location(0, maxY + 1));

                    if (rightCheck || leftCheck || bottomCheck)
                    {
                        return false;
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

        public void Rotate()
        {
            shape = RotateArrayClockwise(shape);
        }

        public void HorizontalTranslation(int dir)
        {
            xSpeed = dir;
            pos.x += xSpeed;
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
