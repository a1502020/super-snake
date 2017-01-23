using DxLibDLL;
using SuperSnake.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnakeServer
{
    public abstract class Phase
    {
        /// <summary>
        /// 1フレームぶんの処理。
        /// </summary>
        /// <returns>次の Phase</returns>
        public Phase Update()
        {
            key.Update();

            var res = this.update();

            DX.ScreenFlip();

            return res;
        }

        /// <summary>
        /// 1フレームぶんの処理。
        /// </summary>
        /// <returns>次の Phase</returns>
        protected abstract Phase update();

        /// <summary>
        /// キー入力
        /// </summary>
        protected static Key key = new Key();

        /// <summary>
        /// ゲームの状態の描画
        /// </summary>
        protected static GameStateDrawer gameStateDrawer = new GameStateDrawer();
    }
}
