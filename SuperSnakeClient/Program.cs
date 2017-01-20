using DxLibDLL;
using SuperSnake.Core;
using SuperSnake.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuperSnakeClient
{
    public class Program
    {
        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        public void Run(string[] args)
        {
            // WebSocket
            var ws = new ClientWebSocket();

            // サーバー接続タスク
            Console.Error.Write("IP:port > ");
            var uriText = Console.ReadLine();
            var taskConnect = ws.ConnectAsync(new Uri("ws://" + uriText), CancellationToken.None);

            // 受信用バッファ
            var buffSize = 1024 * 16;
            var buff = new ArraySegment<byte>(new byte[buffSize]);

            // 受信タスク
            Task<WebSocketReceiveResult> taskReceive = null;

            // DxLib 初期化
            DX.ChangeWindowMode(DX.TRUE);
            DX.SetDoubleStartValidFlag(DX.TRUE);
            DX.DxLib_Init();
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);

            var drawer = new GameStateDrawer();

            // クライアント
            var client = new Clients.KeyboardClient(DX.KEY_INPUT_LEFT, DX.KEY_INPUT_DOWN, DX.KEY_INPUT_RIGHT);
            
            // ゲームの状態
            GameState state = null;
            int myPlayerNum = -1;

            // DxLib メインループ
            while (DX.ProcessMessage() == 0)
            {
                // 接続
                if (taskConnect != null && taskConnect.IsCompleted)
                {
                    taskConnect.Dispose();
                    taskConnect = null;
                }

                // 状態の受信
                if (ws.State == WebSocketState.Open)
                {
                    if (taskReceive == null)
                    {
                        taskReceive = ws.ReceiveAsync(buff, CancellationToken.None);
                    }
                    else if (taskReceive.IsCompleted)
                    {
                        var res = taskReceive.Result;
                        var data = Encoding.UTF8.GetString(buff.Take(res.Count).ToArray());
                        Console.Error.WriteLine("received:");
                        Console.Error.WriteLine(data);
                        using (var reader = new StringReader(data))
                        {
                            var temp = readState(reader);
                            state = temp.Item1;
                            myPlayerNum = temp.Item2;
                        }

                        taskReceive.Dispose();
                        taskReceive = null;
                    }
                }

                // 描画
                if ((object)state != null)
                {
                    drawer.Draw(state);
                }

                DX.ScreenFlip();
            }

            // DxLib 終了
            DX.DxLib_End();
        }

        private Tuple<GameState, int> readState(TextReader reader)
        {
            string line;
            string[] sp;

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

            return Tuple.Create(gameState, myPlayerNum);
        }
    }
}
