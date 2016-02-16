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
        public int Turn { get; private set; }

        public GameState(FieldState field, IList<PlayerState> players, int turn)
        {
            this.Field = field;
            this.Players = players.ToList().AsReadOnly();
            this.Turn = turn;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as GameState);
        }

        public bool Equals(GameState other)
        {
            if ((object)other == null) return false;
            if (!this.Field.Equals(other.Field)) return false;
            if (!this.PlayersCount.Equals(other.PlayersCount)) return false;
            for (var i = 0; i < this.PlayersCount; ++i)
            {
                if (!this.Players[i].Equals(other.Players[i]))
                {
                    return false;
                }
            }
            if (!this.Turn.Equals(other.Turn)) return false;
            return true;
        }

        public static bool operator ==(GameState l, GameState r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(GameState l, GameState r)
        {
            return !l.Equals(r);
        }

        public override int GetHashCode()
        {
            return Field.GetHashCode() ^ Players.GetHashCode();
        }
    }
}
