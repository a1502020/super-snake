using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnake.Core
{
    public class PositionState
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public PositionState(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PositionState);
        }

        public bool Equals(PositionState other)
        {
            if ((object)other == null) return false;
            return this.X.Equals(other.X) && this.Y.Equals(other.Y);
        }

        public static bool operator ==(PositionState l, PositionState r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(PositionState l, PositionState r)
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
