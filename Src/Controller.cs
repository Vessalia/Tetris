using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    class Controller
    {
        private readonly IInput input;
        private Block activeBlock;

        public Controller(IInput input)
        {
            this.input = input;
        }

        public void HandleInput(Grid grid, bool blockRefresh)
        {
            if (input.IsKeyJustPressed(Keys.LeftControl)) { activeBlock.ClampedRotate(grid); }
            if (input.IsKeyJustPressed(Keys.Left)) { activeBlock.HorizontalTranslation(grid, -1); }
            if (input.IsKeyJustPressed(Keys.Right)) { activeBlock.HorizontalTranslation(grid, 1); }
            if (input.IsKeyDown(Keys.Down) && blockRefresh) { activeBlock.VerticalTranslation(1/2); }
        }

        public void SetActiveBlock(Block block)
        {
            activeBlock = block;
        }
    }
}
