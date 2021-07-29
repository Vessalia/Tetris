using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public class Input : IInput
    {
        private KeyboardState prev_state;
        private KeyboardState curr_state;

        public Input()
        {
            curr_state = Keyboard.GetState();
        }

        public void Update()
        {
            prev_state = curr_state;
            curr_state = Keyboard.GetState();
        }


        public bool IsKeyJustPressed(Keys key)
        {
            return prev_state.IsKeyUp(key) && curr_state.IsKeyDown(key);
        }

        public bool IsKeyJustReleased(Keys key)
        {
            return prev_state.IsKeyDown(key) && curr_state.IsKeyUp(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return curr_state.IsKeyUp(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return curr_state.IsKeyDown(key);
        }
    }
}
