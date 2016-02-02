using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSnake.Core;

namespace SuperSnakeStandalone
{
    using Action = SuperSnake.Core.Action;

    public abstract class Client
    {
        public abstract Action Think(GameState gameState, int myPlayerNum);
    }
}
