using SuperSnakeStandalone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSnake.Core;

namespace SuperSnakeStandalone.Clients
{
    using Action = SuperSnake.Core.Action;

    public class RansuchanClient : Client
    {
        public RansuchanClient(Random rnd)
        {
            this.rnd = rnd;
        }

        public override Action Think(GameState gameState, int myPlayerNum)
        {
            // 自分の情報
            var player = gameState.Players[myPlayerNum];
            var pos = player.Position;
            var dir = player.Direction;

            // 1マス先
            var lpos = getNextPos(pos, getLeft(dir));
            var rpos = getNextPos(pos, getRight(dir));
            var spos = getNextPos(pos, dir);

            // 1マス先に行けるか
            var field = gameState.Field;
            var l = isIn(field, lpos) && field.Cells[lpos.X][lpos.Y].Passable;
            var r = isIn(field, rpos) && field.Cells[rpos.X][rpos.Y].Passable;
            var s = isIn(field, spos) && field.Cells[spos.X][spos.Y].Passable;

            // 行ける方向一覧
            var list = new List<Action>();
            if (l) list.Add(Action.Left);
            if (r) list.Add(Action.Right);
            if (s) list.Add(Action.Straight);

            // どこにも行けないなら諦めてランダム
            if (list.Count == 0)
            {
                list = new List<Action> { Action.Left, Action.Right, Action.Straight };
            }

            // ランダムで決定
            return list[rnd.Next(list.Count)];
        }

        private Random rnd;

        private bool isIn(FieldState field, PositionState pos)
        {
            return pos.X >= 0 && pos.Y >= 0 && pos.X < field.Width && pos.Y < field.Height;
        }

        private PositionState getNextPos(PositionState pos, DirectionState dir)
        {
            switch (dir.Value)
            {
                case Direction.Right: return new PositionState(pos.X + 1, pos.Y);
                case Direction.RightUp: return new PositionState(pos.X + 1, pos.Y - 1);
                case Direction.Up: return new PositionState(pos.X, pos.Y - 1);
                case Direction.LeftUp: return new PositionState(pos.X - 1, pos.Y - 1);
                case Direction.Left: return new PositionState(pos.X - 1, pos.Y);
                case Direction.LeftDown: return new PositionState(pos.X - 1, pos.Y + 1);
                case Direction.Down: return new PositionState(pos.X, pos.Y + 1);
                case Direction.RightDown: return new PositionState(pos.X + 1, pos.Y + 1);
            }
            throw new ArgumentException();
        }

        private DirectionState getLeft(DirectionState dir)
        {
            switch (dir.Value)
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
            throw new ArgumentException();
        }

        private DirectionState getRight(DirectionState dir)
        {
            switch (dir.Value)
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
            throw new ArgumentException();
        }
    }
}
