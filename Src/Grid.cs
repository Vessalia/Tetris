using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public enum CellMembers
    {
        empty, block, wall
    }

    public class Grid
    {
        public CellMembers[,] gridValues;

        public Color[,] colours;

        private readonly Location cellMN;

        public Grid(Location cellMN)
        {
            this.cellMN = cellMN;

            BuildGrid(cellMN);
        }

        private void BuildGrid(Location cellMN)
        {

            gridValues = new CellMembers[cellMN.x, cellMN.y];
            colours = new Color[cellMN.x, cellMN.y];

            for (int i = 0; i < cellMN.x; i++)
            {
                for (int j = 0; j < cellMN.y; j++)
                {
                    gridValues[i, j] = CellMembers.empty;

                    colours[i, j] = Color.Transparent;
                }
            }
        }

        public void DrawGrid(SpriteBatch sb)
        {
            int cellLen = GetCellLen();

            var borderPos = Constants.GridToScreenCoords(new Vector2(0, 0), cellMN);

            sb.DrawRectangle(borderPos.X, borderPos.Y, cellLen * cellMN.x, cellLen * cellMN.y, Color.White);

            for (int i = 0; i < cellMN.x; i++)
            {
                for (int j = 0; j < cellMN.y; j++)
                {
                    var screenPos = Constants.GridToScreenCoords(new Vector2(i, j), cellMN);

                    sb.DrawRectangle(screenPos.X, screenPos.Y, cellLen, cellLen, Color.White);

                    sb.FillRectangle(screenPos.X, screenPos.Y, cellLen, cellLen, colours[i, j]);
                }
            }
        }

        public void SetCell(int i, int j, CellMembers cell, Color colour)
        {
            if ((i < cellMN.x && i >= 0) && (j < cellMN.y && j >= 0))
            {
                gridValues[i, j] = cell;
                colours[i, j] = colour;
            }
            else
            {

            }
        }

        public CellMembers GetCell(int i, int j)
        {
            if (i < 0 || j < 0 || i > cellMN.x - 1 || j > cellMN.y - 1)
            {
                return CellMembers.wall;
            }
            return gridValues[i, j];
        }

        public int GetCellLen()
        {
            int minDim;
            int minMN;
            int cellLen;
            if (Constants.Screen.X / cellMN.x >= Constants.Screen.Y / cellMN.y)
            {
                minDim = (int)Constants.Screen.Y;
                minMN = cellMN.y;
            }
            else
            {
                minDim = (int)Constants.Screen.X;
                minMN = cellMN.x;
            }
            return cellLen = (int)MathF.Floor((float)minDim / minMN);
        }

        public Location GetCellNum()
        {
            return cellMN;
        }

        public bool InBounds(Location id)
        {
            return 0 <= id.x && id.x < cellMN.x && 0 <= id.y && id.y < cellMN.y;
        }

        public List<Location> Neighbors(Location cell)
        {
            var neighbors = new List<Location>();

            Location[] DIRS = new[]
            {
                new Location(1, 0),
                new Location(0, -1),
                new Location(-1, 0),
                new Location(0, 1)
            };

            foreach (var dir in DIRS)
            {
                Location next = new Location(cell.x + dir.x, cell.y + dir.y);
                if (InBounds(next) && gridValues[next.x, next.y] == CellMembers.empty)
                {
                    neighbors.Add(next);
                }
            }

            return neighbors;
        }
    }
}
