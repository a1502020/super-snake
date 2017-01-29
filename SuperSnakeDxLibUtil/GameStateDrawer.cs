using DxLibDLL;
using SuperSnake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnake.Util
{
    public class GameStateDrawer
    {
        public GameStateDrawer()
        {
            FieldBasePos = new Position(16, 16);
            FieldRightBottomPos = new Position(640 - 16, 480 - 16);
        }

        /// <summary>
        /// セル(0, 0)の左上の座標
        /// </summary>
        public Position FieldBasePos { get; set; }

        /// <summary>
        /// 最も右下のセルの右下の座標
        /// </summary>
        public Position FieldRightBottomPos { get; set; }

        /// <summary>
        /// プレイヤー一覧の左上の座標
        /// </summary>
        public Position PlayersBasePos { get; set; }

        /// <summary>
        /// ゲームの状態を描画する。
        /// </summary>
        /// <param name="state">ゲームの状態</param>
        public void Draw(GameState state)
        {
            var field = state.Field;
            var players = state.Players;

            var lt = FieldBasePos;
            var rb = FieldRightBottomPos;
            var s1 = (field.Width > 0) ? (rb.X - lt.X) / field.Width : 0;
            var s2 = (field.Height > 0) ? (rb.Y - lt.Y) / field.Height : 0;
            var wh = Math.Min(s1, s2);

            // フィールド
            for (var y = 0; y < field.Height; ++y)
            {
                for (var x = 0; x < field.Width; ++x)
                {
                    var bas = new Position(
                        FieldBasePos.X + x * wh,
                        FieldBasePos.Y + y * wh);
                    var col = field.Cells[x][y].Color;

                    DX.DrawFillBox(
                        bas.X, bas.Y,
                        bas.X + wh, bas.Y + wh,
                        DX.GetColor(col.R, col.G, col.B));
                }
            }

            // フィールド上のプレイヤー
            for (var i = 0; i < players.Count; ++i)
            {
                var pos = players[i].Position;
                var col = players[i].Color;
                var dir = players[i].Direction;
                var angle = 0.0;
                switch (dir.Value)
                {
                    case Direction.Right: angle = 0; break;
                    case Direction.RightUp: angle = 1; break;
                    case Direction.Up: angle = 2; break;
                    case Direction.LeftUp: angle = 3; break;
                    case Direction.Left: angle = 4; break;
                    case Direction.LeftDown: angle = 5; break;
                    case Direction.Down: angle = 6; break;
                    case Direction.RightDown: angle = 7; break;
                }
                angle *= Math.PI / 4;
                var r = Math.Min(wh, wh) / 2 - 1;
                var center = new Position(
                    FieldBasePos.X + pos.X * wh + wh / 2,
                    FieldBasePos.Y + pos.Y * wh + wh / 2);

                DX.DrawCircle(center.X, center.Y, r,
                    DX.GetColor(col.R, col.G, col.B), DX.FALSE);
                DX.DrawLine(center.X, center.Y,
                    (int)(center.X + r * Math.Cos(angle)),
                    (int)(center.Y - r * Math.Sin(angle)),
                    DX.GetColor(col.R, col.G, col.B));
            }

            // プレイヤー一覧
            for (int i = 0; i < state.PlayersCount; i++)
            {
                var basePos = new Position(PlayersBasePos.X, PlayersBasePos.Y + 20 * i);
                var col = state.Players[i].Color;
                DX.DrawFillBox(basePos.X, basePos.Y, basePos.X + 20, basePos.Y + 20, DX.GetColor(255, 255, 255));
                DX.DrawString(basePos.X + 2, basePos.Y + 2, i.ToString(), DX.GetColor(col.R, col.G, col.B));
                DX.DrawString(basePos.X + 22, basePos.Y + 2, state.Players[i].Name, DX.GetColor(255, 255, 255));
            }
        }
    }
}
