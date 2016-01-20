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
        }
    }
}
