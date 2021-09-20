using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public static class Constants
    {
        private static readonly int Width = 1280;
        private static readonly int Height = 720;

        public static Dictionary<string, Keys> keyBindings = new Dictionary<string, Keys> 
        {
            ["left"] = Keys.Left, ["right"] = Keys.Right, ["up"] = Keys.Up, ["down"] = Keys.Down,
            ["rotate cw"] = Keys.X, ["rotate ccw"] = Keys.Z, ["hold"] = Keys.C
        };

        public static readonly Vector2 Screen = new Vector2(Width, Height);

        public static Vector2 GridToScreenCoords(Location gridIndicies, Location cellMN) /* Method needs to be updated for an m =/= n grid */
        {
            var gridPos = new Vector2();
            int minDim;
            int minMN;

            if (Constants.Screen.X / cellMN.x >= Constants.Screen.Y / cellMN.y)
            {
                minDim = (int)Constants.Screen.Y;
                minMN = cellMN.y;
            }
            else
            {
                minDim = (int)Constants.Screen.X;
                minMN = cellMN.x;
            }
            int cellLen = (int)MathF.Floor((float)minDim / minMN);

            gridPos.X = (Constants.Screen.X - cellMN.x * cellLen) / 2 + gridIndicies.x * cellLen;
            gridPos.Y = (Constants.Screen.Y - cellMN.y * cellLen) / 2 + gridIndicies.y * cellLen;

            return gridPos;
        }

        public static void SetKeyBindings(string name, Keys key)
        {
            if (keyBindings.ContainsKey(name))
            {
                keyBindings[name] = key;
            }
            else
            {
                keyBindings.Add(name, key);
            }
        }
    }
}