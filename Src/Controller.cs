using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    class Controller
    {
        private readonly IInput input;

        public Controller(IInput input)
        {
            this.input = input;
        }

        public void HandleInput(Block activeBlock, Grid grid)
        {
            if (input.IsKeyJustPressed(Keys.LeftControl)) { activeBlock.Rotate(grid); }
            if (input.IsKeyJustPressed(Keys.Left)) { activeBlock.HorizontalTranslation(grid, -1); }
            if (input.IsKeyJustPressed(Keys.Right)) { activeBlock.HorizontalTranslation(grid, 1); }
            if (input.IsKeyDown(Keys.Down)) { activeBlock.VerticalTranslation(1/2); }
        }
    }
}
