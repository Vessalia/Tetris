using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    class Block
    {
        private Location pos;

        private bool[,] shape;



        private float timer;
        private float updateTimer;
        private int fallSpeed;

        public Block(Location pos, bool[,] shape)
        {
            this.pos = pos;
            this.shape = shape;

            timer = 0;
            updateTimer = 1;
            fallSpeed = 1;
        }

        public void Draw(Grid grid, SpriteBatch sb)
        {
            Vector2 drawPos = Vector2.Zero;
            for (int i = 0; i < shape.GetLength(1); i++)
            {
                for (int j = 0; j < shape.GetLength(0); j++)
                {
                    if (shape[j, i])
                    {
                        drawPos = Constants.GridToScreenCoords(new Location(pos.x + i, pos.y + j), grid.GetCellMN());
                        sb.FillRectangle(drawPos, new Size2(grid.GetCellLen(), grid.GetCellLen()), Color.White);
                        sb.DrawRectangle(drawPos, new Size2(grid.GetCellLen(), grid.GetCellLen()), Color.Black);
                    }
                }
            }
        }

        public void Update(Grid grid, float dt)
        {
            timer += dt;
            if(timer >= updateTimer && CollisionCheck())
            {
                if (Game1.input.IsKeyDown(Keys.Down))
                {
                    updateTimer = 1/2;
                }
                else
                {
                    updateTimer = 1;
                }

                pos.y += fallSpeed;

                timer = 0;
            }

            if (Game1.input.IsKeyJustPressed(Keys.Left) && CollisionCheck())
            {
                pos.x -= 1;
            }

            if (Game1.input.IsKeyJustPressed(Keys.Right) && CollisionCheck())
            {
                pos.x += 1;
            }
        }

        public bool CollisionCheck()
        {

        }
    }
}
