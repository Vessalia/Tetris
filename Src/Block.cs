using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    class Block
    {
        private Location pos;
        private bool[,] shape;

        public Block(Location pos, bool[,] shape)
        {
            this.pos = pos;
            this.shape = shape;
        }

        public void Update(Grid grid, int updateTimer, int fallSpeed)
        {
            
        }
    }
}
