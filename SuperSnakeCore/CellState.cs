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
        }
    }
}
