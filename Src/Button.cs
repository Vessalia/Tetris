using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public class Button
    {
        private bool wasPressed;

        private Vector2 textSize;
        private Vector2 pos;

        private Color colour;

        private readonly Action action;

        private readonly string text;

        public Button(Vector2 pos, Color colour, string text, Action action)
        {
            textSize = new Vector2();
            wasPressed = false;

            this.pos = pos;
            this.colour = colour;
            this.text = text;
            this.action = action;
        }


        public void DrawButton(SpriteBatch sb, SpriteFont font)
        {
            textSize = font.MeasureString(text);
            sb.FillRectangle((int)pos.X - textSize.X / 2, (int)pos.Y - textSize.Y / 2, (int)textSize.X, (int)textSize.Y, colour);
            sb.DrawString(font, text, pos - textSize / 2, Color.Black);
        }

        public bool MouseButtonCheck()
        {
            return RectangleF.Contains(new RectangleF(pos - textSize / 2, textSize), Mouse.GetState().Position) == true;
        }

        public void OnRelease()
        {
            action();
        }

        public bool GetPressed()
        {
            return wasPressed;
        }

        public void SetPressed(bool wasPressed)
        {
            this.wasPressed = wasPressed;
        }

        public void SetColour(Color colour)
        {
            this.colour = colour;
        }

        public void SetPos(Vector2 newPos)
        {
            pos = newPos;
        }

        public Vector2 GetPos()
        {
            return pos;
        }
    }
}
