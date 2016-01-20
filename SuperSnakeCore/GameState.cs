using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnake.Core
{
    /// <summary>
    /// ### ゲームの状態(game state)
    /// * フィールドの状態とすべてのプレイヤーの状態を合わせたものです。
    /// </summary>
    public class GameState
    {
        public FieldState Field { get; private set; }
        public int PlayersCount { get { return Players.Count; } }
        public IList<PlayerState> Players { get; private set; }

        public GameState(FieldState field, IList<PlayerState> players)
        {
        }
    }
}
