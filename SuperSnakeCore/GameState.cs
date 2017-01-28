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
        public readonly FieldState Field;
        public readonly int PlayersCount;
        public readonly IList<PlayerState> Players;
        public readonly int Turn;
        public readonly bool Finished;
        public readonly PlayerState Winner;
        public readonly int WinnerPlayerNum;

        public GameState(FieldState field, IList<PlayerState> players, int turn)
        {
            this.Field = field;
            this.PlayersCount = players.Count;
            this.Players = players.ToList().AsReadOnly();
            this.Turn = turn;
            this.Finished = players.Count(player => player.Alive) <= 1;
            this.Winner = Finished ? players.FirstOrDefault(player => player.Alive) : null;
            this.WinnerPlayerNum = ((object)this.Winner == null)
                ? -1
                : Enumerable.Range(0, PlayersCount).First(i => players[i] == this.Winner);
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
