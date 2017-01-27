using DxLibDLL;
using SuperSnake.Core;
using SuperSnake.Util;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Action = SuperSnake.Core.Action;

namespace SuperSnakeServer
{
    public class GamePhase : Phase
    {
        public GamePhase(Game game, WebSocketServer server, List<SessionInfo> sessionInfos)
        {
            this.game = game;
            this.server = server;
            this.sessions = sessionInfos;

            playersCount = game.State.PlayersCount;
            tasksSend = Enumerable.Repeat((Task)null, playersCount).ToList();
            decided = Enumerable.Repeat(false, playersCount).ToList();
            actions = Enumerable.Repeat(Action.Straight, playersCount).ToList();
            receiving = Enumerable.Repeat(false, playersCount).ToList();
            received = Enumerable.Repeat(false, playersCount).ToList();
            timeoutCount = Enumerable.Repeat(-1, playersCount).ToList();
            timeoutCountLock = Enumerable.Repeat((object)null, playersCount).ToList();

            server.NewMessageReceived += serverNewMessageReceived;

            gameStateDrawer.FieldBasePos = new Position(2, 22);
            gameStateDrawer.PlayersBasePos = new Position(440, 0);

            beginSend();
        }

        private Game game;
        private WebSocketServer server;
        private List<SessionInfo> sessions;
        private int playersCount;
        private List<Task> tasksSend;
        private List<bool> decided;
        private List<Action> actions;
        private List<bool> receiving, received;
        private List<int> timeoutCount;
        private List<object> timeoutCountLock;
        private int marginSend = 0;
        private bool finished = false;

        protected override Phase update()
        {
            for (int playerNum = 0; playerNum < playersCount; playerNum++)
            {
                // 送信が完了したら行動の受信を開始
                if (tasksSend[playerNum] != null && tasksSend[playerNum].IsCompleted)
                {
                    tasksSend[playerNum].Dispose();
                    tasksSend[playerNum] = null;

                    if (!finished && game.State.Players[playerNum].Alive)
                    {
                        receiving[playerNum] = true;
                        timeoutCount[playerNum] = 10 * 60;
                    }
                }

                // タイムアウト時間計測
                if (timeoutCount[playerNum] > 0)
                {
                    --timeoutCount[playerNum];
                }
            }

            // 結果フェーズへ
            if (finished && tasksSend.All(t => t == null))
            {
                return new ResultPhase(game);
            }

            // すべての生きているプレイヤーの行動を受信したらステップ処理を行う
            if (!finished && (Enumerable.Range(0, playersCount)
                .All(playerNum => !sessions[playerNum].Session.Connected
                    || game.State.Players[playerNum].Dead
                    || received[playerNum]
                    || timeoutCount[playerNum] == 0)))
            {
                game.Step(actions);

                // ゲームが終了したらゲームの状態を送信して結果フェーズへ
                if (game.State.Players.Count(player => player.Alive) <= 1)
                {
                    finished = true;
                    beginSend();
                }

                for (int playerNum = 0; playerNum < playersCount; playerNum++)
                {
                    received[playerNum] = false;
                    actions[playerNum] = Action.Straight;
                }

                // 一定時間待ってからゲームの状態等を送信
                marginSend = 20;
            }

            if (marginSend > 0)
            {
                --marginSend;
                if (marginSend == 0)
                {
                    beginSend();
                }
            }

            // 描画
            DX.DrawFillBox(0, 0, 640, 480, DX.GetColor(0, 0, 0));

            // 各プレイヤーの状態を表示
            var tbl = new int[playersCount, 4];
            for (int i = 0; i < playersCount; i++)
            {
                var conn = sessions[i].Session.Connected;
                var alive = game.State.Players[i].Alive;
                var sending = conn && tasksSend[i] != null;
                tbl[i, 0] = (conn ? 1 : 2);
                tbl[i, 1] = (alive ? 1 : 0);
                tbl[i, 2] = (sending ? 1 : 0);
                tbl[i, 3] = (conn && alive && receiving[i] ? 1 : 0);
            }
            for (int i = 0; i < tbl.GetLength(0); i++)
            {
                var x = 112 + 20 * i;
                var y = 478 - 20 * (tbl.GetLength(1) + 1);
                var str = i.ToString();
                var dx = DX.GetDrawStringWidth(str, str.Length);
                DX.DrawString(x - dx / 2, y, str, DX.GetColor(128, 128, 128));
            }
            for (int j = 0; j < tbl.GetLength(1); j++)
            {
                var x = 98;
                var y = 478 - 20 * (tbl.GetLength(1) - j);
                var str = "";
                if (j == 0) str = "接続";
                if (j == 1) str = "生死";
                if (j == 2) str = "送信中";
                if (j == 3) str = "思考中";
                var dx = DX.GetDrawStringWidth(str, str.Length);
                DX.DrawString(x - dx, y, str, DX.GetColor(128, 128, 128));
            }
            for (int i = 0; i < tbl.GetLength(0); i++)
            {
                for (int j = 0; j < tbl.GetLength(1); j++)
                {
                    var x = 112 + 20 * i;
                    var y = 468 - 20 * (tbl.GetLength(1) - 1 - j);
                    var col = DX.GetColor(0, 0, 0);
                    if (tbl[i, j] == 0) col = DX.GetColor(128, 128, 128);
                    else if (tbl[i, j] == 1) col = DX.GetColor(0, 255, 0);
                    else if (tbl[i, j] == 2) col = DX.GetColor(255, 0, 0);
                    DX.DrawCircle(x, y, 8, col, DX.TRUE);
                }
            }

            gameStateDrawer.Draw(game.State);

            return this;
        }

        private void serverNewMessageReceived(WebSocketSession session, string value)
        {
            for (int playerNum = 0; playerNum < playersCount; playerNum++)
            {
                if (sessions[playerNum].Session != session)
                {
                    continue;
                }

                // 行動を受信
                if (receiving[playerNum])
                {
                    receiving[playerNum] = false;
                    timeoutCount[playerNum] = -1;

                    using (var reader = new StringReader(value))
                    {
                        if (reader.Peek() < 0)
                        {
                            actions[playerNum] = Action.Straight;
                        }
                        var line = reader.ReadLine();
                        if (line == "Straight") actions[playerNum] = Action.Straight;
                        else if (line == "Left") actions[playerNum] = Action.Left;
                        else if (line == "Right") actions[playerNum] = Action.Right;
                        else actions[playerNum] = Action.Straight;
                    }

                    received[playerNum] = true;
                }
            }
        }

        private void beginSend()
        {
            for (int playerNum = 0; playerNum < playersCount; playerNum++)
            {
                if (!sessions[playerNum].Session.Connected)
                {
                    continue;
                }

                // 送信する文字列
                var sb = new StringBuilder();
                var state = new GameStateText
                {
                    GameState = game.State,
                    MyPlayerNum = playerNum,
                    Finished = finished,
                };
                using (var writer = new StringWriter(sb))
                {
                    state.Write(writer);
                }
                var str = sb.ToString();

                // 必要なバッファサイズ
                var size = Encoding.UTF8.GetByteCount(str) + 1;

                var s = sessions[playerNum].Session;
                tasksSend[playerNum] = Task.Run(() =>
                {
                    s.Send(size.ToString());
                    Thread.Sleep(50);
                    s.Send(str);
                });
            }
        }
    }
}
