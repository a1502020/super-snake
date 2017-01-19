using DxLibDLL;
using SuperSnake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnake.Net.Core
{
    public class GameStateDrawer
    {
        public GameStateDrawer()
        {
            BasePos = new Position(16, 16);
            CellSize = new Size(16, 16);
        }

        /// <summary>
        /// ゲームの状態を描画する。
        /// </summary>
        /// <param name="state">ゲームの状態</param>
        public void Draw(GameState state)
        {
            var field = state.Field;
            var players = state.Players;

            // フィールド
            for (var y = 0; y < field.Height; ++y)
            {
                for (var x = 0; x < field.Width; ++x)
                {
                    var bas = new Position(
                        BasePos.X + x * CellSize.Width,
                        BasePos.Y + y * CellSize.Height);
                    var col = field.Cells[x][y].Color;

                    DX.DrawFillBox(
                        bas.X, bas.Y,
                        bas.X + CellSize.Width, bas.Y + CellSize.Height,
                        DX.GetColor(col.R, col.G, col.B));
                }
            }

            // プレイヤー
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
                var r = Math.Min(CellSize.Width, CellSize.Height) / 2 - 1;
                var center = new Position(
                    BasePos.X + pos.X * CellSize.Width + CellSize.Width / 2,
                    BasePos.Y + pos.Y * CellSize.Height + CellSize.Height / 2);

                DX.DrawCircle(center.X, center.Y, r,
                    DX.GetColor(col.R, col.G, col.B), DX.FALSE);
                DX.DrawLine(center.X, center.Y,
                    (int)(center.X + r * Math.Cos(angle)),
                    (int)(center.Y - r * Math.Sin(angle)),
                    DX.GetColor(col.R, col.G, col.B));
            }
        }

        /// <summary>
        /// (0, 0)の左上の座標
        /// </summary>
        public Position BasePos { get; set; }

        /// <summary>
        /// セルを描画する大きさ
        /// </summary>
        public Size CellSize { get; set; }
    }
}
