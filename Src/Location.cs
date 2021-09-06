using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris.Src
{
    public struct Location
    {
        public int x, y;
        public static Location zero = new Location(0, 0);

        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Location (Location location)
        {
            x = location.x;
            y = location.y;
        }

        public static bool operator ==(Location a, Location b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Location a, Location b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public static Location operator +(Location a, Location b)
        {
            return new Location(a.x + b.x, a.y + b.y);
        }

        public static Location operator -(Location a, Location b)
        {
            return new Location(a.x - b.x, a.y - b.y);
        }

        public static Location operator *(Location a, Location b)
        {
            return new Location(a.x * b.x, a.y * b.y);
        }

        public static Location operator /(Location a, Location b)
        {
            return new Location(a.x / b.x, a.y / b.y);
        }

        public static Location Zero()
        {
            return zero;
        }
    }
}
