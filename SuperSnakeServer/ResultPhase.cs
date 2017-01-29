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

            // 結果を表示
            var str = (game.State.WinnerPlayerNum < 0)
                ? "引き分け"
                : string.Format("プレイヤー{0}: {1} の勝ち！",
                    game.State.WinnerPlayerNum, game.State.Winner.Name);
            DX.DrawString(2, 2, str, DX.GetColor(255, 255, 255));

            gameStateDrawer.Draw(game.State);

            return this;
        }
    }
}
