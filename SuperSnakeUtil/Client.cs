using SuperSnake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = SuperSnake.Core.Action;

namespace SuperSnake.Util
{
    public abstract class Client
    {
        /// <summary>
        /// ゲームの状態を元にプレイヤーの行動を決定する。
        /// 各クライアントが実装しなければならない。
        /// </summary>
        /// <param name="gameState">ゲームの状態</param>
        /// <param name="myPlayerNum">対応するプレイヤーのプレイヤー番号</param>
        /// <returns>行動</returns>
        public abstract Action Think(GameState gameState, int myPlayerNum);

        /// <summary>
        /// プレイヤーの名前
        /// </summary>
        public virtual string Name
        {
            get { return "プレイヤー"; }
        }

        /// <summary>
        /// 与えられた位置から与えられた方向に進んだ次の位置を求める。
        /// 例えば (1, 2), Right を渡すと (2, 2) を返す。
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="dir">向き</param>
        /// <returns>次の位置</returns>
        public static PositionState GetNextPosition(PositionState pos, DirectionState dir)
        {
            switch (dir.Value)
            {
                case Direction.Right: return new PositionState(pos.X + 1, pos.Y);
                case Direction.RightUp: return new PositionState(pos.X + 1, pos.Y - 1);
                case Direction.Up: return new PositionState(pos.X, pos.Y - 1);
                case Direction.LeftUp: return new PositionState(pos.X - 1, pos.Y - 1);
                case Direction.Left: return new PositionState(pos.X - 1, pos.Y);
                case Direction.LeftDown: return new PositionState(pos.X - 1, pos.Y + 1);
                case Direction.Down: return new PositionState(pos.X, pos.Y + 1);
                case Direction.RightDown: return new PositionState(pos.X + 1, pos.Y + 1);
            }
            throw new ArgumentException();
        }

        /// <summary>
        /// 与えられた向きを左(反時計回り)に45度回転した向きを取得する。
        /// </summary>
        /// <param name="dir">向き</param>
        /// <returns>左に45度回転した向き</returns>
        public static DirectionState GetLeft(DirectionState dir)
        {
            switch (dir.Value)
            {
                case Direction.Right: return new DirectionState(Direction.RightUp);
                case Direction.RightUp: return new DirectionState(Direction.Up);
                case Direction.Up: return new DirectionState(Direction.LeftUp);
                case Direction.LeftUp: return new DirectionState(Direction.Left);
                case Direction.Left: return new DirectionState(Direction.LeftDown);
                case Direction.LeftDown: return new DirectionState(Direction.Down);
                case Direction.Down: return new DirectionState(Direction.RightDown);
                case Direction.RightDown: return new DirectionState(Direction.Right);
            }
            throw new ArgumentException();
        }

        /// <summary>
        /// 与えられた向きを右(時計回り)に45度回転した向きを取得する。
        /// </summary>
        /// <param name="dir">向き</param>
        /// <returns>右に45度回転した向き</returns>
        public static DirectionState GetRight(DirectionState dir)
        {
            switch (dir.Value)
            {
                case Direction.Right: return new DirectionState(Direction.RightDown);
                case Direction.RightUp: return new DirectionState(Direction.Right);
                case Direction.Up: return new DirectionState(Direction.RightUp);
                case Direction.LeftUp: return new DirectionState(Direction.Up);
                case Direction.Left: return new DirectionState(Direction.LeftUp);
                case Direction.LeftDown: return new DirectionState(Direction.Left);
                case Direction.Down: return new DirectionState(Direction.LeftDown);
                case Direction.RightDown: return new DirectionState(Direction.Down);
            }
            throw new ArgumentException();
        }

        /// <summary>
        /// 与えられた位置がフィールド内か否かを調べる。
        /// </summary>
        /// <param name="field">フィールド</param>
        /// <param name="pos">位置</param>
        /// <returns>位置がフィールド内なら true 、そうでなければ false</returns>
        public static bool IsIn(FieldState field, PositionState pos)
        {
            return pos.X >= 0 && pos.Y >= 0 && pos.X < field.Width && pos.Y < field.Height;
        }

        /// <summary>
        /// 乱数
        /// </summary>
        protected static Random rnd = new Random();
    }
}
