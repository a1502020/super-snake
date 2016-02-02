using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSnake.Core;

namespace SuperSnakeStandalone
{
    public class InitialGameState
    {
        public FieldState Field { get; private set; }
        public IList<InitialPlayerState> Players { get; private set; }
        public int PlayersCount { get { return Players.Count; } }

        public InitialGameState(FieldState field, IList<InitialPlayerState> players)
        {
            this.Field = field;
            this.Players = players.ToList().AsReadOnly();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as InitialGameState);
        }

        public bool Equals(InitialGameState other)
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
            return true;
        }

        public static bool operator ==(InitialGameState l, InitialGameState r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(InitialGameState l, InitialGameState r)
        {
            return !l.Equals(r);
        }

        public override int GetHashCode()
        {
            return Field.GetHashCode() ^ Players.GetHashCode();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} {1}", Field.Width, Field.Height);
            sb.AppendLine();
            for (var y = 0; y < Field.Height; ++y)
            {
                for (var x = 0; x < Field.Width; ++x)
                {
                    sb.Append(Field.Cells[x][y].Passable ? 'o' : 'x');
                }
                for (var x = 0; x < Field.Width; ++x)
                {
                    sb.AppendFormat(" {0}", Field.Cells[x][y].Color.ToString());
                }
                sb.AppendLine();
            }
            sb.AppendFormat("{0}", PlayersCount);
            sb.AppendLine();
            for (var i = 0; i < PlayersCount; ++i)
            {
                sb.AppendLine(Players[i].ToString());
            }
            return sb.ToString();
        }
    }
}
