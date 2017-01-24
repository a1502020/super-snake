using DxLibDLL;
using SuperSnake.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnakeClient
{
    public class ResultPhase : Phase
    {
        public ResultPhase(GameStateText state)
        {
            this.state = state;
        }

        private GameStateText state;

        protected override Phase update()
        {
            // 描画
            DX.DrawFillBox(0, 0, 640, 480, DX.GetColor(0, 0, 0));

            gameStateDrawer.Draw(state.GameState);

            // 自分のプレイヤー番号と色
            var col = state.GameState.Players[state.MyPlayerNum].Color;
            var str = state.MyPlayerNum.ToString();
            var strw = DX.GetDrawStringWidth(str, str.Length);
            DX.DrawFillBox(0, 20, 4 + strw, 40, DX.GetColor(255, 255, 255));
            DX.DrawString(2, 22, state.MyPlayerNum.ToString(), DX.GetColor(col.R, col.G, col.B));

            return this;
        }
    }
}
