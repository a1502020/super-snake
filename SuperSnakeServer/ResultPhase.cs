using DxLibDLL;
using SuperSnake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnakeServer
{
    public class ResultPhase : Phase
    {
        public ResultPhase(Game game)
        {
            this.game = game;
        }

        private Game game;

        protected override Phase update()
        {
            // 描画
            DX.DrawFillBox(0, 0, 640, 480, DX.GetColor(0, 0, 0));

            // 各プレイヤーの色と名前を表示
            for (int i = 0; i < game.State.PlayersCount; i++)
            {
                var x = Math.Max(440, 4 + gameStateDrawer.BasePos.X + gameStateDrawer.CellSize.Width * game.State.Field.Width);
                var y = 4 + 20 * i;
                var strNum = i.ToString();
                var dx = DX.GetDrawStringWidth(strNum, strNum.Length);
                DX.DrawFillBox(x - 2, y - 2, x + dx + 2, y + 20, DX.GetColor(255, 255, 255));
                var col = game.State.Players[i].Color;
                DX.DrawString(x, y, strNum, DX.GetColor(col.R, col.G, col.B));
                DX.DrawString(x + dx + 4, y, game.State.Players[i].Name, DX.GetColor(255, 255, 255));
            }

            var str = "引き分け";
            for (int playerNum = 0; playerNum < game.State.PlayersCount; playerNum++)
            {
                if (game.State.Players[playerNum].Alive)
                {
                    str = string.Format("プレイヤー{0}: {1} の勝ち！",
                        playerNum, game.State.Players[playerNum].Name);
                }
            }
            DX.DrawString(2, 2, str, DX.GetColor(255, 255, 255));

            gameStateDrawer.Draw(game.State);

            return this;
        }
    }
}
