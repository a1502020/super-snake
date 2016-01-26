using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnake.Core
{
    public class Game
    {
        public Game(GameState initState)
        {
            this.State = initState;
        }

        public GameState State { get; private set; }

        public void Step(IList<Action> actions)
        {
        }
    }
}
