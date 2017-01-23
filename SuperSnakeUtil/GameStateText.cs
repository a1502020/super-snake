using SuperSnake.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnake.Util
{
    public class GameStateText
    {
        /// <summary>
        /// ゲームの状態
        /// </summary>
        public GameState GameState;

        /// <summary>
        /// 自分のプレイヤー番号
        /// </summary>
        public int MyPlayerNum;

        /// <summary>
        /// ゲームが終了したか否か
        /// </summary>
        public bool Finished;

        /// <summary>
        /// 文字列ストリームから読み込む。
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public void Read(TextReader reader)
        {
            string line;
            string[] sp;

            // ゲームが終了したか否か
            if (reader.Peek() < 0) throw new FormatException();
            line = reader.ReadLine();
            if (line == "Continue") Finished = false;
            else if (line == "Finished") Finished = true;
            else throw new FormatException();

            // フィールドの名前
            if (reader.Peek() < 0) throw new FormatException();
            line = reader.ReadLine();
            var fieldName = line;

            // 現在のターン数(0-based)
            if (reader.Peek() < 0) throw new FormatException();
            line = reader.ReadLine();
            var turn = int.Parse(line);
            if (turn < 0) throw new FormatException();

            // フィールドの幅、高さ
            if (reader.Peek() < 0) throw new FormatException();
            line = reader.ReadLine();
            sp = line.Split(' ');
            if (sp.Length != 2) throw new FormatException();
            var w = int.Parse(sp[0]);
            var h = int.Parse(sp[1]);

            // フィールド
            var tempField = new List<string[]>();
            for (int y = 0; y < h; y++)
            {
                if (reader.Peek() < 0) throw new FormatException();
                line = reader.ReadLine();
                sp = line.Split(' ');
                if (sp.Length != w * 4) throw new FormatException();
                tempField.Add(sp);
            }
            var cells = new List<IList<CellState>>();
            for (int x = 0; x < w; x++)
            {
                var cellsLine = new List<CellState>();
                for (int y = 0; y < h; y++)
                {
                    if (tempField[y][x * 4] != "0" && tempField[y][x * 4] != "1") throw new FormatException();
                    var passable = (tempField[y][x * 4] == "0");
                    var r = int.Parse(tempField[y][x * 4 + 1]);
                    if (r < 0 || r >= 256) throw new FormatException();
                    var g = int.Parse(tempField[y][x * 4 + 2]);
                    if (g < 0 || g >= 256) throw new FormatException();
                    var b = int.Parse(tempField[y][x * 4 + 3]);
                    if (b < 0 || b >= 256) throw new FormatException();
                    var col = new ColorState(r, g, b);
                    cellsLine.Add(new CellState(col, passable));
                }
                cells.Add(cellsLine);
            }

            // プレイヤー数
            if (reader.Peek() < 0) throw new FormatException();
            line = reader.ReadLine();
            var playersCount = int.Parse(line);

            // プレイヤー
            var players = new List<PlayerState>();
            for (int playerNum = 0; playerNum < playersCount; playerNum++)
            {
                // 名前
                if (reader.Peek() < 0) throw new FormatException();
                line = reader.ReadLine();
                var name = line;

                // 色
                if (reader.Peek() < 0) throw new FormatException();
                line = reader.ReadLine();
                sp = line.Split(' ');
                if (sp.Length != 3) throw new FormatException();
                var r = int.Parse(sp[0]);
                if (r < 0 || r >= 256) throw new FormatException();
                var g = int.Parse(sp[1]);
                if (g < 0 || g >= 256) throw new FormatException();
                var b = int.Parse(sp[2]);
                if (b < 0 || b >= 256) throw new FormatException();
                var col = new ColorState(r, g, b);

                // 位置、向き、生死
                if (reader.Peek() < 0) throw new FormatException();
                line = reader.ReadLine();
                sp = line.Split(' ');
                if (sp.Length != 4) throw new FormatException();

                // 位置
                var x = int.Parse(sp[0]);
                var y = int.Parse(sp[1]);
                var pos = new PositionState(x, y);

                // 向き
                Direction tempDir;
                if (sp[2] == "Right") tempDir = Direction.Right;
                else if (sp[2] == "RightUp") tempDir = Direction.RightUp;
                else if (sp[2] == "Up") tempDir = Direction.Up;
                else if (sp[2] == "LeftUp") tempDir = Direction.LeftUp;
                else if (sp[2] == "Left") tempDir = Direction.Left;
                else if (sp[2] == "LeftDown") tempDir = Direction.LeftDown;
                else if (sp[2] == "Down") tempDir = Direction.Down;
                else if (sp[2] == "RightDown") tempDir = Direction.RightDown;
                else throw new FormatException();
                var dir = new DirectionState(tempDir);

                // 生死
                if (sp[3] != "Alive" && sp[3] != "Dead") throw new FormatException();
                var alive = (sp[3] == "Alive");

                players.Add(new PlayerState(playerNum, name, col, pos, dir, alive));
            }

            // 自分のプレイヤー番号(0-based)
            if (reader.Peek() < 0) throw new FormatException();
            line = reader.ReadLine();
            var myPlayerNum = int.Parse(line);

            var fieldState = new FieldState(fieldName, cells);
            var gameState = new GameState(fieldState, players, turn);

            this.GameState = gameState;
            this.MyPlayerNum = myPlayerNum;
        }

        /// <summary>
        /// 文字列ストリームに書き込む。
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="data"></param>
        public void Write(TextWriter writer)
        {
            writer.WriteLine(Finished ? "Finished" : "Continue");
            writer.WriteLine(GameState.Field.Name);
            writer.WriteLine(GameState.Turn.ToString());
            writer.WriteLine(string.Format("{0} {1}", GameState.Field.Width, GameState.Field.Height));
            for (int y = 0; y < GameState.Field.Height; y++)
            {
                for (int x = 0; x < GameState.Field.Width; x++)
                {
                    var cell = GameState.Field.Cells[x][y];
                    if (x != 0) writer.Write(' ');
                    writer.Write("{0} {1} {2} {3}",
                        cell.Passable ? 0 : 1,
                        cell.Color.R, cell.Color.G, cell.Color.B);
                }
                writer.WriteLine();
            }
            writer.WriteLine(string.Format("{0}", GameState.PlayersCount));
            for (int playerNum = 0; playerNum < GameState.PlayersCount; playerNum++)
            {
                var p = GameState.Players[playerNum];
                writer.WriteLine(p.Name);
                writer.WriteLine(string.Format("{0} {1} {2}",
                    p.Color.R, p.Color.G, p.Color.B));
                writer.Write("{0} {1} ", p.Position.X, p.Position.Y);
                switch (p.Direction.Value)
                {
                    case Direction.Right: writer.Write("Right"); break;
                    case Direction.RightUp: writer.Write("RightUp"); break;
                    case Direction.Up: writer.Write("Up"); break;
                    case Direction.LeftUp: writer.Write("LeftUp"); break;
                    case Direction.Left: writer.Write("Left"); break;
                    case Direction.LeftDown: writer.Write("LeftDown"); break;
                    case Direction.Down: writer.Write("Down"); break;
                    case Direction.RightDown: writer.Write("RightDown"); break;
                }
                writer.WriteLine(p.Alive ? " Alive" : " Dead");
            }
            writer.WriteLine(MyPlayerNum.ToString());
        }
    }
}
