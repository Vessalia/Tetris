using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    class GhostBlock
    {
        private readonly Block block;
        private readonly Grid grid;

        private bool[,] Shape;

        private Location pos;

        public GhostBlock(Block block, Grid grid)
        {
            this.block = block;
            this.grid = grid;

            Shape = block.Shape;

            pos = new Location(block.GetPos().x, 0);
        }

        public void Update()
        {
            Shape = block.Shape;
            pos.x = block.GetPos().x;
            pos.y = GetPosY();
        }

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < Shape.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < Shape.GetUpperBound(1) + 1; j++)
                {
                    if (Shape[j, i])
                    {
                        Vector2 drawPos = Constants.GridToScreenCoords(new Location(pos.x + i, pos.y + j), grid.cellMN);
                        sb.DrawRectangle(drawPos, new Size2(grid.GetCellLen(), grid.GetCellLen()), Color.LightBlue, 10);
                    }
                }
            }
        }

        private int GetPosY()
        {
            pos.y = 0;

            while (!CollisionCheck())
            {
                if (!BlockCollisionCheck())
                {
                    pos.y += 1;
                }
                else
                {
                    break;
                }
            }

            pos.y -= 1;

            int blockPosY = block.GetPos().y;

            if (blockPosY > pos.y)
            {
                pos.y = blockPosY;
            }

            return pos.y;
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

        private bool CollisionCheck()
        {
            for (int i = 0; i < Shape.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < Shape.GetUpperBound(1) + 1; j++)
                {
                    if (Shape[j, i])
                    {
                        if (!grid.InBounds(new Location(pos.x + i, pos.y + j)))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
