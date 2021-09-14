﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private float timer;
        private float blockClearTimer;

        public Grid(Location cellMN)
        {
            this.cellMN = cellMN;

            BuildGrid(cellMN);

            timer = 0;
            blockClearTimer = 1 / 6f;
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

                    //if(j == cellMN.y - 1 || j == cellMN.y - 2 || j == cellMN.y - 4 || j == cellMN.y - 6 || j == cellMN.y - 7 || j == cellMN.y - 8 || j == cellMN.y - 9 || j == cellMN.y - 11 || j == cellMN.y - 12 || j == cellMN.y - 13)
                    //{
                    //    gridValues[i, j] = CellMembers.block;

                    //    colours[i, j] = Color.Red;
                    //}

                    //if (j == cellMN.y - 3 && i != cellMN.x - 1)
                    //{
                    //    gridValues[i, j] = CellMembers.block;

                    //    colours[i, j] = Color.Red;
                    //}

                    //if (j == cellMN.y - 5 && i != cellMN.x - 2)
                    //{
                    //    gridValues[i, j] = CellMembers.block;

                    //    colours[i, j] = Color.Red;
                    //}

                    //if (j == cellMN.y - 10 && i != cellMN.x - 3)
                    //{
                    //    gridValues[i, j] = CellMembers.block;

                    //    colours[i, j] = Color.Red;
                    //}

                    //if (j == cellMN.y - 14 && i != cellMN.x - 4)
                    //{
                    //    gridValues[i, j] = CellMembers.block;

                    //    colours[i, j] = Color.Red;
                    //}
                }
            }
        }

        public void DrawGrid(SpriteBatch sb)
        {
            int cellLen = GetCellLen();

            var borderPos = Constants.GridToScreenCoords(new Location(0, 0), cellMN);

            sb.DrawRectangle(borderPos.X, borderPos.Y, cellLen * cellMN.x, cellLen * cellMN.y, Color.White);

            for (int i = 0; i < cellMN.x; i++)
            {
                for (int j = 0; j < cellMN.y; j++)
                {
                    var screenPos = Constants.GridToScreenCoords(new Location(i, j), cellMN);

                    sb.DrawRectangle(screenPos.X, screenPos.Y, cellLen, cellLen, Color.White);

                    sb.FillRectangle(screenPos.X, screenPos.Y, cellLen, cellLen, colours[i, j]);

                    if (gridValues[i, j] != CellMembers.empty)
                    {
                        sb.DrawRectangle(screenPos.X, screenPos.Y, cellLen, cellLen, Color.Black);
                    }
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
            return (int)MathF.Floor((float)minDim / minMN);
        }

        public Location GetCellMN()
        {
            return cellMN;
        }

        public bool InBounds(Location id)
        {
            return 0 <= id.x && id.x < cellMN.x && 0 <= id.y && id.y < cellMN.y;
        }

        public void CheckLines(float dt)
        {
            var lines = new List<int>();
            for (int j = 0; j < cellMN.y; j++)
            {
                int row = 0;
                for (int i = 0; i < cellMN.x; i++)
                {
                    if (gridValues[i, j] == CellMembers.block)
                    {
                        row += 1;
                    }
                }

                if (row == cellMN.x)
                {
                    lines.Add(j);
                }
            }

            if(lines.Count > 0)
            {
                lines.Sort();
                lines.Reverse();
                HandleLines(lines, dt);
            }
        }

        private void HandleLines(List<int> lines, float dt)
        {
            int clearedLines = 0;

            timer += dt;

            if(timer > blockClearTimer)
            {
                for (int j = cellMN.y - 1; j >= 0; j--)
                {
                    while (lines.Contains(j - clearedLines))
                    {
                        clearedLines++;
                    }

                    int shiftedRow = j - clearedLines;
                    if (shiftedRow >= 0)
                    {
                        for (int i = 0; i < cellMN.x; i++)
                        {
                            gridValues[i, j] = gridValues[i, shiftedRow];
                            colours[i, j] = colours[i, shiftedRow];
                        }
                    }
                    else
                    {
                        for (int i = 0; i < cellMN.x; i++)
                        {
                            gridValues[i, j] = CellMembers.empty;
                            colours[i, j] = Color.Transparent;
                        }
                    }
                }

                timer = 0;
            }
            else
            {
                for (int j = cellMN.y; j >= 0; j--)
                {
                    if (lines.Contains(j))
                    {
                        for (int i = 0; i < cellMN.x; i++)
                        {
                            colours[i, j] = Color.White;
                        }
                    }
                }
            }
        }

        public bool IsCellEmpty(Location cell)
        {
            if (gridValues[cell.x, cell.y] != CellMembers.empty)
            {
                return false;
            }

            return true;
        }

        public void PlaceBlock(Block block)
        {
            var shape = block.GetShape();

            for (int i = 0; i < shape.GetUpperBound(0) + 1; i++)
            {
                int gridY = i + block.GetPos().y;

                for (int j = 0; j < shape.GetUpperBound(1) + 1; j++)
                {
                    if (shape[i, j])
                    {
                        int gridX = j + block.GetPos().x;

                        gridValues[gridX, gridY] = CellMembers.block;
                        colours[gridX, gridY] = block.GetColour();
                    }
                }
            }
        }
    }
}
