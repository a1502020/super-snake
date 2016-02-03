using SuperSnake.Core;
using SuperSnakeStandalone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = SuperSnake.Core.Action;

namespace SuperSnakeStandalone.Clients
{
    public class RansuchanClient : Client
    {
        public override Action Think(GameState gameState, int myPlayerNum)
        {
            // 自分の情報
            var player = gameState.Players[myPlayerNum];
            var pos = player.Position;
            var dir = player.Direction;

            // 1マス先
            var lpos = GetNextPosition(pos, GetLeft(dir));
            var rpos = GetNextPosition(pos, GetRight(dir));
            var spos = GetNextPosition(pos, dir);

            // 1マス先に行けるか
            var field = gameState.Field;
            var l = IsIn(field, lpos) && field.Cells[lpos.X][lpos.Y].Passable;
            var r = IsIn(field, rpos) && field.Cells[rpos.X][rpos.Y].Passable;
            var s = IsIn(field, spos) && field.Cells[spos.X][spos.Y].Passable;

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
    }
}
