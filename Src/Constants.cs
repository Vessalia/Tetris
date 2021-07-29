using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public static class Constants
    {
        private static readonly int Width = 1280;
        private static readonly int Height = 720;

        public static readonly Vector2 Screen = new Vector2(Width, Height);

        public static Vector2 GridToScreenCoords(Vector2 gridIndicies, int cellNum) /* Method needs to be updated for an m =/= n grid */
        {
            var gridPos = new Vector2();
            int minDim = (int)MathF.Round(MathF.Min(Constants.Screen.X, Constants.Screen.Y));
            int cellLen = (int)MathF.Floor((float)minDim / cellNum);

            float cellFloor = (float)minDim / cellNum - (int)MathF.Floor((float)minDim / cellNum);

            gridPos.X = cellFloor * cellNum / 2 + (Constants.Screen.X - Constants.Screen.Y) / 2 + gridIndicies.X * cellLen;
            gridPos.Y = cellFloor * cellNum / 2 + gridIndicies.Y * cellLen;

            return gridPos;
        }
    }
}