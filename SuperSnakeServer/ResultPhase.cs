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

            gameStateDrawer.Draw(game.State);

            DX.DrawString(2, 2, "おしまい", DX.GetColor(255, 255, 255));

            return this;
        }
    }
}
