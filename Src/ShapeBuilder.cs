using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    static class ShapeBuilder
    {
        private static bool[,] iShape =
            {
                {false, true, false, false },
                {false, true, false, false },
                {false, true, false, false },
                {false, true, false, false }
            };

        private static bool[,] oShape =
            {
                {false, false, false, false },
                {false, true, true, false },
                {false, true, true, false },
                {false, false, false, false }
            };

        private static bool[,] tShape =
            {
                {false, false, false, false },
                {false, true, true, true },
                {false, false, true, false },
                {false, false, true, false }
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
                {false, false, true, false },
                {false, false, true, false },
                {false, false, true, false },
                {false, true, true, false }
            };

        private static bool[,] lShape =
            {
                {false, true, false, false },
                {false, true, false, false },
                {false, true, false, false },
                {false, true, false, false }
            };

        public static Block CreateIBlock(Location pos)
        {
            return new Block(pos, iShape);
        }
        public static Block CreateOBlock(Location pos)
        {
            return new Block(pos, oShape);
        }
        public static Block CreateTBlock(Location pos)
        {
            return new Block(pos, tShape);
        }
        public static Block CreateSBlock(Location pos)
        {
            return new Block(pos, sShape);
        }
        public static Block CreateZBlock(Location pos)
        {
            return new Block(pos, zShape);
        }
        public static Block CreateJBlock(Location pos)
        {
            return new Block(pos, jShape);
        }
        public static Block CreateILBlock(Location pos)
        {
            return new Block(pos, lShape);
        }
    }
}
