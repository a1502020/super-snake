using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSnake.Core;

namespace SuperSnake.Util
{
    public class InitialPlayerState
    {
        public PositionState Position { get; private set; }
        public DirectionState Direction { get; private set; }

        public InitialPlayerState(PositionState position, DirectionState direction)
        {
            this.Position = position;
            this.Direction = direction;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as InitialPlayerState);
        }

        public bool Equals(InitialPlayerState other)
        {
            if ((object)other == null) return false;
            return this.Position.Equals(other.Position) && this.Direction.Equals(other.Direction);
        }

        public static bool operator ==(InitialPlayerState l, InitialPlayerState r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(InitialPlayerState l, InitialPlayerState r)
        {
            return !l.Equals(r);
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode() ^ Direction.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Position.X, Position.Y, Direction.ToString());
        }
    }
}
