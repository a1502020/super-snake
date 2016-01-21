using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnake.Core
{
    /// <summary>
    /// ### フィールドの状態(field state)
    /// * フィールドの名前と幅、高さ、フィールド上のすべてのセルの状態を合わせたものです。
    /// </summary>
    public class FieldState
    {
        public string Name { get; private set; }
        public int Width { get { return Cells.Count; } }
        public int Height { get { return (Width == 0) ? 0 : Cells[0].Count; } }
        public IList<IList<CellState>> Cells { get; private set; }

        public FieldState(string name, IList<IList<CellState>> cells)
        {
            this.Name = name;
            var tempCells = new List<IList<CellState>>();
            foreach (var row in cells)
            {
                tempCells.Add(row.ToList().AsReadOnly());
            }
            this.Cells = tempCells.AsReadOnly();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as FieldState);
        }

        public bool Equals(FieldState other)
        {
            if ((object)other == null) return false;
            if (!this.Name.Equals(other.Name)) return false;
            if (!this.Width.Equals(other.Width) || !this.Height.Equals(other.Height)) return false;
            for (var x = 0; x < this.Width; ++x)
            {
                for (var y = 0; y < this.Height; ++y)
                {
                    if (!this.Cells[x][y].Equals(other.Cells[x][y]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool operator ==(FieldState l, FieldState r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(FieldState l, FieldState r)
        {
            return !l.Equals(r);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Cells.GetHashCode();
        }

        public override string ToString()
        {
            var res = new StringBuilder();
            res.AppendLine(Name);
            for (var y = 0; y < Height; ++y)
            {
                for (var x = 0; x < Width; ++x)
                {
                    res.Append(Cells[x][y].Passable ? 'o' : 'x');
                }
                res.AppendLine();
            }
            return res.ToString();
        }
    }
}
