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

        /// <summary>
        /// 右に４５度回転した向き
        /// </summary>
        public DirectionState Right
        {
            get
            {
                switch (Value)
                {
                    case Direction.Right: return new DirectionState(Direction.RightDown);
                    case Direction.RightUp: return new DirectionState(Direction.Right);
                    case Direction.Up: return new DirectionState(Direction.RightUp);
                    case Direction.LeftUp: return new DirectionState(Direction.Up);
                    case Direction.Left: return new DirectionState(Direction.LeftUp);
                    case Direction.LeftDown: return new DirectionState(Direction.Left);
                    case Direction.Down: return new DirectionState(Direction.LeftDown);
                    case Direction.RightDown: return new DirectionState(Direction.Down);
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// 左に４５度回転した向き
        /// </summary>
        public DirectionState Left
        {
            get
            {
                switch (Value)
                {
                    case Direction.Right: return new DirectionState(Direction.RightUp);
                    case Direction.RightUp: return new DirectionState(Direction.Up);
                    case Direction.Up: return new DirectionState(Direction.LeftUp);
                    case Direction.LeftUp: return new DirectionState(Direction.Left);
                    case Direction.Left: return new DirectionState(Direction.LeftDown);
                    case Direction.LeftDown: return new DirectionState(Direction.Down);
                    case Direction.Down: return new DirectionState(Direction.RightDown);
                    case Direction.RightDown: return new DirectionState(Direction.Right);
                }
                throw new InvalidOperationException();
            }
        }

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
