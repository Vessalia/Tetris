using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public class Menu
    {
        private readonly List<Button> buttons;

        public Menu()
        {
            buttons = new List<Button>();
        }


        public void DrawButtons(SpriteBatch sb, SpriteFont font)
        {
            foreach (var b in buttons)
            {
                b.DrawButton(sb, font);
            }
        }


        public void AddButton(Vector2 pos, Color colour, string text, Action action)
        {
            buttons.Add(new Button(pos, colour, text, action));
        }

        public void HandleButtonInput()
        {
            foreach (var b in buttons)
            {
                if (b.MouseButtonCheck())
                {
                    b.SetColour(Color.Red);
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        b.SetColour(Color.Green);
                        b.SetPressed(true);
                    }
                    else if ((Mouse.GetState().LeftButton == ButtonState.Released) && b.GetPressed())
                    {
                        b.OnRelease();
                        b.SetPressed(false);
                        break;
                    }
                }
                else
                {
                    b.SetColour(Color.White);
                    b.SetPressed(false);
                }
            }
        }
    }
}
