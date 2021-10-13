using Microsoft.Xna.Framework;
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

        public Location CellMN { get; }

        private float timer;
        private readonly float blockClearTimer;

        public bool Clearing { get; private set; }

        public int Score { get; private set; }
        private int level;

        public Grid(Location cellMN)
        {
            CellMN = cellMN;

            BuildGrid(cellMN);

            timer = 0;
            blockClearTimer = 1 / 6f;

            Clearing = false;

            Score = 0;
            level = 1;
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

            var borderPos = Constants.GridToScreenCoords(new Location(0, 0), CellMN);

            sb.DrawRectangle(borderPos.X, borderPos.Y, cellLen * CellMN.x, cellLen * CellMN.y, Color.White);

            for (int i = 0; i < CellMN.x; i++)
            {
                for (int j = 0; j < CellMN.y; j++)
                {
                    var screenPos = Constants.GridToScreenCoords(new Location(i, j), CellMN);

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
            if ((i < CellMN.x && i >= 0) && (j < CellMN.y && j >= 0))
            {
                gridValues[i, j] = cell;
                colours[i, j] = colour;
            }
        }

        public CellMembers GetCell(int i, int j)
        {
            if (i < 0 || j < 0 || i > CellMN.x - 1 || j > CellMN.y - 1)
            {
                return CellMembers.wall;
            }
            return gridValues[i, j];
        }

        public int GetCellLen()
        {
            int minDim;
            int minMN;
            if (Constants.Screen.X / CellMN.x >= Constants.Screen.Y / CellMN.y)
            {
                minDim = (int)Constants.Screen.Y;
                minMN = CellMN.y;
            }
            else
            {
                minDim = (int)Constants.Screen.X;
                minMN = CellMN.x;
            }
            return (int)MathF.Floor((float)minDim / minMN);
        }

        public bool InBounds(Location id)
        {
            return 0 <= id.x && id.x < CellMN.x && 0 <= id.y && id.y < CellMN.y;
        }

        public void CheckLines(float dt)
        {
            var lines = new List<int>();
            for (int j = 0; j < CellMN.y; j++)
            {
                int row = 0;
                for (int i = 0; i < CellMN.x; i++)
                {
                    if (gridValues[i, j] == CellMembers.block)
                    {
                        row += 1;
                    }
                }

                if (row == CellMN.x)
                {
                    lines.Add(j);
                }
            }

            if(lines.Count > 0)
            {
                HandleLines(lines, dt);
            }
        }

        private void HandleLines(List<int> lines, float dt)
        {
            int clearedLines = 0;

            timer += dt;

            if(timer > blockClearTimer)
            {
                for (int j = CellMN.y - 1; j >= 0; j--)
                {
                    while (lines.Contains(j - clearedLines))
                    {
                        clearedLines++;
                    }

                    int shiftedRow = j - clearedLines;
                    if (shiftedRow >= 0)
                    {
                        for (int i = 0; i < CellMN.x; i++)
                        {
                            gridValues[i, j] = gridValues[i, shiftedRow];
                            colours[i, j] = colours[i, shiftedRow];
                        }
                    }
                    else
                    {
                        for (int i = 0; i < CellMN.x; i++)
                        {
                            gridValues[i, j] = CellMembers.empty;
                            colours[i, j] = Color.Transparent;
                        }
                    }
                }

                timer = 0;
                clearing = false;

                score += ScoreGained(lines);
                level += lines.Count;
            }
            else
            {
                for (int j = CellMN.y; j >= 0; j--)
                {
                    if (lines.Contains(j))
                    {
                        for (int i = 0; i < CellMN.x; i++)
                        {
                            colours[i, j] = Color.White;
                        }
                    }
                }

                clearing = true;
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
            var shape = block.Shape;

            for (int i = 0; i < shape.GetUpperBound(0) + 1; i++)
            {
                int gridY = i + block.GetPos().y;

                for (int j = 0; j < shape.GetUpperBound(1) + 1; j++)
                {
                    if (shape[i, j])
                    {
                        int gridX = j + block.GetPos().x;

                        gridValues[gridX, gridY] = CellMembers.block;
                        colours[gridX, gridY] = block.Colour;
                    }
                }
            }
        }

        private int ScoreGained(List<int> lines)
        {
            if (lines.Count == 1)
            {
                return 40;
            }
            else if (lines.Count == 2)
            {
                return 100;
            }
            else if (lines.Count == 3)
            {
                return 300;
            }
            else if (lines.Count == 4)
            {
                return 1200;
            }
            else
            {
                return 0;
            }
        }

        public int GetLevel()
        {
            return  1 + level / 10;
        }
    }
}
