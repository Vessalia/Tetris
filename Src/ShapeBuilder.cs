using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    static class ShapeBuilder
    {
        private static readonly bool[,] iShape =
            {
                {false, true, false, false },
                {false, true, false, false },
                {false, true, false, false },
                {false, true, false, false }
            };

        private static readonly bool[,] oShape =
            {
                {false, false, false, false },
                {false, true, true, false },
                {false, true, true, false },
                {false, false, false, false }
            };

        private static readonly bool[,] tShape =
            {
                {false, false, false, false },
                {false, true, true, true },
                {false, false, true, false },
                {false, false, false, false }
            };

        private static bool[,] sShape =
            {
                {false, false, false, false },
                {false, false, true, true },
                {false, true, true, false },
                {false, false, false, false }
            };

        private static bool[,] zShape =
            {
                {false, false, false, false },
                {false, true, true, false },
                {false, false, true, true },
                {false, false, false, false }
            };

        private static bool[,] jShape =
            {
                {false, false, false, false },
                {false, false, true, false },
                {false, false, true, false },
                {false, true, true, false }
            };

        private static bool[,] lShape =
            {
                {false, false, false, false },
                {false, true, false, false },
                {false, true, false, false },
                {false, true, true, false }
            };

        private static readonly Color iColour = new Color(0, 255, 255);
        private static readonly Color oColour = new Color(255, 255, 0);
        private static readonly Color tColour = new Color(128, 0, 128);
        private static readonly Color sColour = new Color(0, 255, 0);
        private static readonly Color zColour = new Color(255, 0, 0);
        private static readonly Color jColour = new Color(0, 0, 255);
        private static readonly Color lColour = new Color(255, 127, 0);

        public static Block CreateIBlock(Location pos, Grid grid)
        {
            return new Block(pos, iShape, iColour, grid);
        }

        public static Block CreateOBlock(Location pos, Grid grid)
        {
            return new Block(pos, oShape, oColour, grid);
        }

        public static Block CreateTBlock(Location pos, Grid grid)
        {
            return new Block(pos, tShape, tColour, grid);
        }

        public static Block CreateSBlock(Location pos, Grid grid)
        {
            return new Block(pos, sShape, sColour, grid);
        }

        public static Block CreateZBlock(Location pos, Grid grid)
        {
            return new Block(pos, zShape, zColour, grid);
        }

        public static Block CreateJBlock(Location pos, Grid grid)
        {
            return new Block(pos, jShape, jColour, grid);
        }

        public static Block CreateLBlock(Location pos, Grid grid)
        {
            return new Block(pos, lShape, lColour, grid);
        }
    }
}
