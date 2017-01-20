using DxLibDLL;
using SuperSnake.Core;
using SuperSnake.Util;
using SuperSocket.SocketBase.Config;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = SuperSnake.Core.Action;

namespace SuperSnakeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        public void Run(string[] args)
        {
            // サーバー設定
            var server = new WebSocketServer();
            var rootConfig = new RootConfig();
            var serverConfig = new ServerConfig()
            {
                Ip = "Any",
                Port = 30304,
                MaxConnectionNumber = 4,
                Mode = SuperSocket.SocketBase.SocketMode.Tcp,
                Name = "Super Snake Server",
            };

            // DxLib 初期化
            DX.ChangeWindowMode(DX.TRUE);
            DX.DxLib_Init();
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);

            var drawer = new GameStateDrawer();

            // ゲームの初期状態
            var igsReader = new InitialGameStateReader();
            var initState = igsReader.Read("fields/test.txt");

            // プレイヤー(仮)
            var playerInfos = new List<PlayerInfo>
            {
                new PlayerInfo("Player 1", new ColorState(255, 0, 0)),
                new PlayerInfo("Player 2", new ColorState(0, 0, 255)),
                new PlayerInfo("Player 3", new ColorState(255, 255, 0)),
                new PlayerInfo("Player 4", new ColorState(0, 255, 0)),
            };

            // プレイヤーの行動(仮)
            var actions = new List<Action>
            {
                Action.Straight,
                Action.Straight,
                Action.Straight,
                Action.Straight,
            };

            // ゲーム
            var game = new Game(igsReader.Combine(initState, playerInfos));

            // クライアントセッション
            var sessionInfos = new List<SessionInfo>();

            // クライアント接続時
            server.NewSessionConnected += s =>
            {
                sessionInfos.Add(new SessionInfo(s, sessionInfos.Count));
            };

            // クライアント切断時
            server.SessionClosed += (s, reason) =>
            {
                sessionInfos.RemoveAll(sinfo => sinfo.Session == s);
            };

            // サーバー起動
            server.Setup(rootConfig, serverConfig);
            server.Start();

            // キー入力
            var key = new Key();

            // DxLib メインループ
            while (DX.ProcessMessage() == 0)
            {
                // キー入力
                key.Update();

                // エンターキーでクライアント全員にゲームの状態を送信(仮)
                if (key.IsPressed(DX.KEY_INPUT_RETURN))
                {
                    foreach (var sinfo in sessionInfos)
                    {
                        var message = writeState(game.State, sinfo.PlayerNum);
                        sinfo.TaskSendGameState = sinfo.Send(message);
                    }
                }

                // 描画
                if ((object)game != null)
                {
                    drawer.Draw(game.State);
                }

                DX.ScreenFlip();
            }

            // サーバー終了
            server.Stop();

            // DxLib 終了
            DX.DxLib_End();
        }

        private string writeState(GameState state, int myPlayerNum)
        {
            var sb = new StringBuilder();
            sb.AppendLine(state.Field.Name);
            sb.AppendLine(state.Turn.ToString());
            sb.AppendLine(string.Format("{0} {1}", state.Field.Width, state.Field.Height));
            for (int y = 0; y < state.Field.Height; y++)
            {
                for (int x = 0; x < state.Field.Width; x++)
                {
                    var cell = state.Field.Cells[x][y];
                    if (x != 0) sb.Append(' ');
                    sb.AppendFormat("{0} {1} {2} {3}",
                        cell.Passable ? 0 : 1,
                        cell.Color.R, cell.Color.G, cell.Color.B);
                }
                sb.AppendLine();
            }
            sb.AppendLine(string.Format("{0}", state.PlayersCount));
            for (int playerNum = 0; playerNum < state.PlayersCount; playerNum++)
            {
                var p = state.Players[playerNum];
                sb.AppendLine(p.Name);
                sb.AppendLine(string.Format("{0} {1} {2}",
                    p.Color.R, p.Color.G, p.Color.B));
                sb.AppendFormat("{0} {1} ", p.Position.X, p.Position.Y);
                switch (p.Direction.Value)
                {
                    case Direction.Right: sb.Append("Right"); break;
                    case Direction.RightUp: sb.Append("RightUp"); break;
                    case Direction.Up: sb.Append("Up"); break;
                    case Direction.LeftUp: sb.Append("LeftUp"); break;
                    case Direction.Left: sb.Append("Left"); break;
                    case Direction.LeftDown: sb.Append("LeftDown"); break;
                    case Direction.Down: sb.Append("Down"); break;
                    case Direction.RightDown: sb.Append("RightDown"); break;
                }
                sb.AppendLine(p.Alive ? " Alive" : " Dead");
            }
            sb.AppendLine(myPlayerNum.ToString());
            return sb.ToString();
        }
    }
}
