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
            makeCellsUnderPlayersNotPassable();
        }

        public GameState State { get; private set; }

        public void Step(IList<Action> actions)
        {
            if (actions == null)
            {
                throw new ArgumentNullException();
            }
            if (actions.Count != State.PlayersCount)
            {
                throw new ArgumentException();
            }

            // 移動フェーズ
            stepMove(actions);

            // 死亡判定フェーズ
            stepJudge();

            // すべてのプレイヤーの位置のセルを通行可能でなくする
            makeCellsUnderPlayersNotPassable();
        }

        private void stepMove(IList<Action> actions)
        {
            var cells = State.Field.Cells.Select(row => row.ToList()).ToList();
            for (var i = State.PlayersCount - 1; i >= 0; --i)
            {
                var player = State.Players[i];
                var pos = player.Position;
                if (isInField(pos) && player.Alive)
                {
                    cells[pos.X][pos.Y] = new CellState(player.Color, cells[pos.X][pos.Y].Passable);
                }
                stepMovePlayer(i, actions[i]);
            }
            State = new GameState(
                new FieldState(
                    State.Field.Name,
                    cells.Select(row => (IList<CellState>)row.AsReadOnly()).ToList().AsReadOnly()
                ),
                State.Players);
        }

        private void stepMovePlayer(int playerNumber, Action action)
        {
            var player = State.Players[playerNumber];
            var dir = player.Direction.Value;
            if (player.Dead)
            {
                return;
            }
            if (action == Action.Right)
            {
                switch (dir)
                {
                    case Direction.Right: dir = Direction.RightDown; break;
                    case Direction.RightUp: dir = Direction.Right; break;
                    case Direction.Up: dir = Direction.RightUp; break;
                    case Direction.LeftUp: dir = Direction.Up; break;
                    case Direction.Left: dir = Direction.LeftUp; break;
                    case Direction.LeftDown: dir = Direction.Left; break;
                    case Direction.Down: dir = Direction.LeftDown; break;
                    case Direction.RightDown: dir = Direction.Down; break;
                }
            }
            else if (action == Action.Left)
            {
                switch (dir)
                {
                    case Direction.Right: dir = Direction.RightUp; break;
                    case Direction.RightUp: dir = Direction.Up; break;
                    case Direction.Up: dir = Direction.LeftUp; break;
                    case Direction.LeftUp: dir = Direction.Left; break;
                    case Direction.Left: dir = Direction.LeftDown; break;
                    case Direction.LeftDown: dir = Direction.Down; break;
                    case Direction.Down: dir = Direction.RightDown; break;
                    case Direction.RightDown: dir = Direction.Right; break;
                }
            }
            var pos = player.Position;
            switch (dir)
            {
                case Direction.Right: pos = new PositionState(pos.X + 1, pos.Y); break;
                case Direction.RightUp: pos = new PositionState(pos.X + 1, pos.Y - 1); break;
                case Direction.Up: pos = new PositionState(pos.X, pos.Y - 1); break;
                case Direction.LeftUp: pos = new PositionState(pos.X - 1, pos.Y - 1); break;
                case Direction.Left: pos = new PositionState(pos.X - 1, pos.Y); break;
                case Direction.LeftDown: pos = new PositionState(pos.X - 1, pos.Y + 1); break;
                case Direction.Down: pos = new PositionState(pos.X, pos.Y + 1); break;
                case Direction.RightDown: pos = new PositionState(pos.X + 1, pos.Y + 1); break;
            }
            var players = State.Players.ToList();
            players[playerNumber] = new PlayerState(
                player.Number, player.Name, player.Color,
                pos, new DirectionState(dir), player.Alive);
            State = new GameState(State.Field, players.AsReadOnly());
        }

        private void stepJudge()
        {
            for (var i = 0; i < State.PlayersCount; ++i)
            {
                var player = State.Players[i];
                if (player.Dead)
                {
                    continue;
                }
                if (stepJudgePlayer(i))
                {
                    var players = State.Players.ToList();
                    players[i] = new PlayerState(
                        player.Number, player.Name, player.Color,
                        player.Position, player.Direction, false);
                    State = new GameState(State.Field, players.AsReadOnly());
                }
            }
        }

        private bool stepJudgePlayer(int playerNumber)
        {
            var player = State.Players[playerNumber];
            if (!isInField(player.Position))
            {
                return true;
            }
            if (!State.Field.Cells[player.Position.X][player.Position.Y].Passable)
            {
                return true;
            }
            for (var i = 0; i < State.PlayersCount; ++i)
            {
                if (i != playerNumber && State.Players[i].Position == player.Position)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// すべてのプレイヤーの位置のセルを通行可能でなくする。
        /// </summary>
        private void makeCellsUnderPlayersNotPassable()
        {
            var cells = State.Field.Cells.Select(row => row.ToList()).ToList();
            foreach (var player in State.Players.Where(player => isInField(player.Position)))
	        {
                var x = player.Position.X;
                var y = player.Position.Y;
                cells[x][y] = new CellState(cells[x][y].Color, false);
	        }
            State = new GameState(
                new FieldState(
                    State.Field.Name,
                    cells.Select(row => (IList<CellState>)row.AsReadOnly()).ToList()
                ),
                State.Players);
        }

        private bool isInField(PositionState pos)
        {
            return pos.X >= 0 && pos.Y >= 0 && pos.X < State.Field.Width && pos.Y < State.Field.Height;
        }
    }
}
