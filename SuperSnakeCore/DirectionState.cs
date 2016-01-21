using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnake.Core
{
    public class DirectionState
    {
        public Direction Value { get; private set; }

        public DirectionState(Direction value)
        {
            this.Value = value;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as DirectionState);
        }

        public bool Equals(DirectionState other)
        {
            if ((object)other == null) return false;
            return this.Value.Equals(other.Value);
        }

        public static bool operator ==(DirectionState l, DirectionState r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(DirectionState l, DirectionState r)
        {
            return !l.Equals(r);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
