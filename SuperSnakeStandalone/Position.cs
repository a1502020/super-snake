using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnakeStandalone
{
    public class Position
    {
        public Position()
        {
            X = 0;
            Y = 0;
        }

        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Position);
        }

        public bool Equals(Position other)
        {
            if ((object)other == null) return false;
            return this.X.Equals(other.X) && this.Y.Equals(other.Y);
        }

        public static bool operator ==(Position l, Position r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(Position l, Position r)
        {
            return !l.Equals(r);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }
    }
}
