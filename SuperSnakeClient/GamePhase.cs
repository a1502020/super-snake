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
using Action = SuperSnake.Core.Action;

namespace SuperSnakeClient
{
    public class GamePhase : Phase
    {
        public GamePhase(ClientWebSocket ws, Client client)
        {
            this.ws = ws;
            this.client = client;

            buffSize = 1024;
            buff = new ArraySegment<byte>(new byte[buffSize]);

            gameStateDrawer.FieldBasePos = new Position(2, 42);
            gameStateDrawer.PlayersBasePos = new Position(440, 0);

            client.MessageSending += sendMessage;

            taskReceiveSize = ws.ReceiveAsync(buff, CancellationToken.None);
        }

        private ClientWebSocket ws;
        private Client client;
        private Task<WebSocketReceiveResult> taskReceiveSize = null;
        private Task<WebSocketReceiveResult> taskReceiveState = null;
        private int buffSize;
        private ArraySegment<byte> buff;
        private GameStateText state = new GameStateText();
        private Task<Action> taskThink = null;
        private Task taskSend = null;
        private Queue<string> messageQueue = new Queue<string>();
        private Task taskSendMessage = null;

        private int cnt = 0;

        protected override Phase update()
        {
            // 必要バッファサイズを受信する
            if (taskReceiveSize != null && taskReceiveSize.IsCompleted)
            {
                var res = taskReceiveSize.Result;
                taskReceiveSize.Dispose();
                taskReceiveSize = null;

                if (isMessage(res, buff))
                {
                    var temp = readMessage(res, buff);
                    client.MessageReceived(temp.Item1, temp.Item2);

                    taskReceiveSize = ws.ReceiveAsync(buff, CancellationToken.None);
                }
                else
                {
                    // タイムアウトなら前回の思考タスクを無視
                    taskThink = null;

                    var size = int.Parse(Encoding.UTF8.GetString(buff.Take(res.Count).ToArray()));
                    if (size > buffSize)
                    {
                        buffSize = size;
                        buff = new ArraySegment<byte>(new byte[buffSize]);
                    }

                    taskReceiveState = ws.ReceiveAsync(buff, CancellationToken.None);
                }
            }

            // ゲームの状態と自分のプレイヤー番号を受信
            if (taskReceiveState != null && taskReceiveState.IsCompleted)
            {
                var res = taskReceiveState.Result;
                taskReceiveState.Dispose();
                taskReceiveState = null;

                if (isMessage(res, buff))
                {
                    var temp = readMessage(res, buff);
                    client.MessageReceived(temp.Item1, temp.Item2);

                    taskReceiveState = ws.ReceiveAsync(buff, CancellationToken.None);
                }
                else
                {
                    var data = Encoding.UTF8.GetString(buff.Take(res.Count).ToArray());
                    using (var reader = new StringReader(data))
                    {
                        if (reader.Peek() < 0 || reader.ReadLine() != "GameState")
                        {
                            throw new FormatException();
                        }
                        state.Read(reader);
                    }

                    // ゲームが終了したら結果フェーズへ
                    if (state.Finished)
                    {
                        return new ResultPhase(state);
                    }

                    // プレイヤーが死んでいなければ思考開始
                    if (!state.Finished && state.GameState.Players[state.MyPlayerNum].Alive)
                    {
                        taskThink = Task.Run(() => client.Think(state.GameState, state.MyPlayerNum));
                    }

                    taskReceiveSize = ws.ReceiveAsync(buff, CancellationToken.None);
                }
            }

            // 思考結果を送信する
            if (taskThink != null && taskThink.IsCompleted && taskSendMessage == null)
            {
                var res = taskThink.Result;
                taskThink.Dispose();
                taskThink = null;

                var text = "Straight";
                switch (res)
                {
                    case Action.Straight: text = "Straight"; break;
                    case Action.Right: text = "Right"; break;
                    case Action.Left: text = "Left"; break;
                }
                var data = new ArraySegment<byte>(Encoding.UTF8.GetBytes(text));
                taskSend = ws.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
            }

            // 送信完了
            if (taskSend != null && taskSend.IsCompleted)
            {
                taskSend.Dispose();
                taskSend = null;
            }

            // メッセージ送信
            if (messageQueue.Count != 0 && taskSendMessage == null && taskSend == null)
            {
                var mes = messageQueue.Dequeue();
                var str = string.Format("Message" + Environment.NewLine + "{0}", mes);
                var data = new ArraySegment<byte>(Encoding.UTF8.GetBytes(str));
                taskSendMessage = ws.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            if (taskSendMessage != null && taskSendMessage.IsCompleted)
            {
                taskSendMessage.Dispose();
                taskSendMessage = null;
            }

            // 描画
            DX.DrawFillBox(0, 0, 640, 480, DX.GetColor(0, 0, 0));

            if ((object)state.GameState != null)
            {
                gameStateDrawer.Draw(state.GameState);

                // 自分のプレイヤー番号と色
                var col = state.GameState.Players[state.MyPlayerNum].Color;
                var str = state.MyPlayerNum.ToString();
                var strw = DX.GetDrawStringWidth(str, str.Length);
                DX.DrawFillBox(0, 20, 4 + strw, 40, DX.GetColor(255, 255, 255));
                DX.DrawString(2, 22, state.MyPlayerNum.ToString(), DX.GetColor(col.R, col.G, col.B));
            }

            // 状態
            var dots = new string('.', cnt / 30);
            if ((taskReceiveSize != null || taskReceiveState != null) && taskThink == null)
            {
                DX.DrawString(2, 2,
                    string.Format("受信待ち中{0}", dots),
                    DX.GetColor(255, 255, 255));
            }
            else if (taskThink != null)
            {
                DX.DrawString(2, 2,
                    string.Format("思考中{0}", dots),
                    DX.GetColor(255, 255, 255));
            }
            else if (taskSend != null)
            {
                DX.DrawString(2, 2,
                    string.Format("送信中{0}", dots),
                    DX.GetColor(255, 255, 255));
            }

            ++cnt;
            if (cnt >= 120) cnt = 0;

            return this;
        }

        private void sendMessage(string message)
        {
            messageQueue.Enqueue(message);
        }

        private bool isMessage(WebSocketReceiveResult res, ArraySegment<byte> buff)
        {
            var str = Encoding.UTF8.GetString(buff.Take(res.Count).ToArray());
            using (var reader = new StringReader(str))
            {
                if (reader.Peek() < 0) return false;
                var line = reader.ReadLine();
                return line == "Message";
            }
        }

        private Tuple<int, string> readMessage(WebSocketReceiveResult res, ArraySegment<byte> buff)
        {
            var str = Encoding.UTF8.GetString(buff.Take(res.Count).ToArray());
            using (var reader = new StringReader(str))
            {
                if (reader.Peek() < 0) throw new FormatException();
                var line = reader.ReadLine();
                if (line != "Message") throw new FormatException();

                if (reader.Peek() < 0) throw new FormatException();
                line = reader.ReadLine();
                var playerNum = int.Parse(line);

                if (reader.Peek() < 0) throw new FormatException();
                line = reader.ReadLine();
                var mes = line;

                return Tuple.Create(playerNum, mes);
            }
        }
    }
}
