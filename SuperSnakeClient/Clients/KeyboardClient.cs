using DxLibDLL;
using SuperSnake.Core;
using SuperSnake.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = SuperSnake.Core.Action;

namespace SuperSnakeClient.Clients
{
    public class KeyboardClient : Client
    {
        public KeyboardClient(int keyStraight, int keyLeft, int keyRight)
        {
            this.keys = keyStraight;
            this.keyl = keyLeft;
            this.keyr = keyRight;
        }

        public override Action Think(GameState gameState, int myPlayerNum)
        {
            while (true)
            {
                if (DX.CheckHitKey(keys) != 0) return Action.Straight;
                if (DX.CheckHitKey(keyl) != 0) return Action.Left;
                if (DX.CheckHitKey(keyr) != 0) return Action.Right;
            }
        }

        public override string Name
        {
            get
            {
                return "手動プレイヤー";
            }
        }

        private int keys, keyl, keyr;
    }
}
