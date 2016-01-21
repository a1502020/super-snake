using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnake.Core
{
    /// <summary>
    /// ### プレイヤーの状態(player state)
    /// * プレイヤー番号とプレイヤーの名前、色、位置、向き、生死を合わせたものです。
    /// </summary>
    public class PlayerState
    {
        public int Number { get; private set; }
        public string Name { get; private set; }
        public ColorState Color { get; private set; }
        public PositionState Position { get; private set; }
        public DirectionState Direction { get; private set; }
        public bool Alive { get; private set; }
        public bool Dead { get { return !Alive; } }

        public PlayerState(
            int number, string name, ColorState color,
            PositionState position, DirectionState direction,
            bool alive)
        {
            this.Number = number;
            this.Name = name;
            this.Color = color;
            this.Position = position;
            this.Direction = direction;
            this.Alive = alive;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PlayerState);
        }

        public bool Equals(PlayerState other)
        {
            if ((object)other == null) return false;
            return
                this.Number.Equals(other.Number)
                && this.Name.Equals(other.Name)
                && this.Color.Equals(other.Color)
                && this.Position.Equals(other.Position)
                && this.Direction.Equals(other.Direction)
                && this.Alive.Equals(other.Alive);
        }

        public static bool operator ==(PlayerState l, PlayerState r)
        {
            return l.Equals(r);
        }

        public static bool operator !=(PlayerState l, PlayerState r)
        {
            return !l.Equals(r);
        }

        public override int GetHashCode()
        {
            return
                Number.GetHashCode()
                ^ Name.GetHashCode()
                ^ Color.GetHashCode()
                ^ Position.GetHashCode()
                ^ Direction.GetHashCode()
                ^ Alive.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(
                "PlayerState({0}, \"{1}\", {2}, {3}, {4}, {5})",
                Number, Name, Color, Position, Direction, Alive ? "Alive" : "Dead");
        }
    }
}
