using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnake.Core
{
    /// <summary>
    /// ### セルの状態(cell state)
    /// * セルの色と通行可能か否かを合わせたものです。
    /// </summary>
    public class CellState
    {
        public ColorState Color { get; private set; }
        public bool Passable { get; private set; }

        public CellState(ColorState color, bool passable)
        {
            this.Color = color;
            this.Passable = passable;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as CellState);
        }

        public bool Equals(CellState other)
        {
            if ((object)other == null) return false;
            return this.Color.Equals(other.Color) && this.Passable.Equals(other.Passable);
        }

        public static bool operator ==(CellState l, CellState r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(CellState l, CellState r)
        {
            return !l.Equals(r);
        }

        public override int GetHashCode()
        {
            return Color.GetHashCode() ^ Passable.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", Passable ? 'o' : 'x', Color);
        }
    }
}
