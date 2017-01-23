using DxLibDLL;
using SuperSnake.Core;
using SuperSnake.Util;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnakeServer
{
    public class ConnectionPhase : Phase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="server">SetUp 済みのサーバー</param>
        public ConnectionPhase(WebSocketServer server, InitialGameState initState)
        {
            this.server = server;
            this.initState = initState;

            this.server.NewSessionConnected += serverNewSessionConnected;
            this.server.SessionClosed += serverSessionClosed;
            this.server.NewMessageReceived += serverNewMessageReceived;

            this.server.Start();
        }

        private WebSocketServer server;
        private InitialGameState initState;
        private List<SessionInfo> sessions = new List<SessionInfo>();
        private List<ColorState> defaultColors = new List<ColorState>
        {
            new ColorState(255, 0, 0),
            new ColorState(0, 0, 255),
            new ColorState(192, 192, 0),
            new ColorState(0, 255, 0),
        };

        private int cnt = 0;

        protected override Phase update()
        {
            // 最大人数接続するかエンターキーが押されたらゲームフェーズに移行する
            if (sessions.Count == initState.PlayersCount || key.IsPressed(DX.KEY_INPUT_RETURN))
            {
                var playerInfos = sessions
                    .Select((si, i) => new PlayerInfo(si.PlayerName, defaultColors[i % defaultColors.Count]))
                    .ToList();
                var game = new Game(InitialGameStateReader.Combine(initState, playerInfos));

                // すべてのサーバーイベントを削除
                server.NewSessionConnected -= serverNewSessionConnected;
                server.SessionClosed -= serverSessionClosed;
                server.NewMessageReceived -= serverNewMessageReceived;

                return new GamePhase(game, server, sessions);
            }

            // 描画
            DX.DrawFillBox(0, 0, 640, 480, DX.GetColor(0, 0, 0));
            var dots = new string('.', cnt / 30);

            DX.DrawString(2, 2, string.Format("接続受付中{0}", dots), DX.GetColor(255, 255, 255));
            for (int playerNum = 0; playerNum < sessions.Count; playerNum++)
            {
                DX.DrawString(2, 22 + playerNum * 20, sessions[playerNum].PlayerName, DX.GetColor(255, 255, 255));
            }

            ++cnt;
            if (cnt >= 120) cnt = 0;

            return this;
        }

        private void serverNewSessionConnected(WebSocketSession session)
        {
            // クライアントが接続された
            var si = new SessionInfo(session);
            si.PlayerName = "プレイヤー";
            sessions.Add(si);
        }

        private void serverSessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            // クライアントが切断された
            sessions.RemoveAll(si => si.Session == session);
        }

        private void serverNewMessageReceived(WebSocketSession session, string value)
        {
            // エントリー情報を受信
            foreach (var si in sessions)
            {
                if (si.Session != session)
                {
                    continue;
                }
                using (var reader = new StringReader(value))
                {
                    if (reader.Peek() < 0) break;
                    si.PlayerName = reader.ReadLine();
                }
                break;
            }
        }
    }
}
