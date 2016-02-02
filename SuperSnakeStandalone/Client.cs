using SuperSnake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = SuperSnake.Core.Action;

namespace SuperSnakeStandalone
{
    public abstract class Client
    {
        public abstract Action Think(GameState gameState, int myPlayerNum);
    }
}
