using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    interface IInput
    {
        public bool IsKeyJustPressed(Keys key);

        public bool IsKeyJustReleased(Keys key);

        public bool IsKeyUp(Keys key);

        public bool IsKeyDown(Keys key);
    }
}
