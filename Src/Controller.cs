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
            if (input.IsKeyJustPressed(Constants.keyBindings["rotate cw"])) { activeBlock.ClampedRotateClockwise(grid); }
            if (input.IsKeyJustPressed(Constants.keyBindings["rotate ccw"])) { activeBlock.ClampedRotateCounterClockwise(grid); }
            if (input.IsKeyJustPressed(Constants.keyBindings["left"])) { activeBlock.HorizontalTranslation(grid, -1); }
            if (input.IsKeyJustPressed(Constants.keyBindings["right"])) { activeBlock.HorizontalTranslation(grid, 1); }
            if (input.IsKeyDown(Constants.keyBindings["down"]) && blockRefresh) { activeBlock.VerticalTranslation(1/2); }
        }

        public void SetActiveBlock(Block block)
        {
            activeBlock = block;
        }
    }
}
