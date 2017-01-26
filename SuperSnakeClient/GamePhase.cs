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

            buffSize = 1024 * 16;
            buff = new ArraySegment<byte>(new byte[buffSize]);

            gameStateDrawer.FieldBasePos = new Position(2, 42);
            gameStateDrawer.PlayersBasePos = new Position(440, 0);
        }

        private ClientWebSocket ws;
        private Client client;
        private Task<WebSocketReceiveResult> taskReceive = null;
        private int buffSize;
        private ArraySegment<byte> buff;
        private GameStateText state = new GameStateText();
        private Task<Action> taskThink = null;
        private Task taskSend = null;

        private int cnt = 0;

        protected override Phase update()
        {
            // ゲームの状態と自分のプレイヤー番号を受信する
            if (taskReceive == null && taskThink == null && taskSend == null)
            {
                taskReceive = ws.ReceiveAsync(buff, CancellationToken.None);
            }
            if (taskReceive != null && taskReceive.IsCompleted)
            {
                var res = taskReceive.Result;
                taskReceive.Dispose();
                taskReceive = null;

                var data = Encoding.UTF8.GetString(buff.Take(res.Count).ToArray());
                using (var reader = new StringReader(data))
                {
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
            }

            // 思考結果を送信する
            if (taskThink != null && taskThink.IsCompleted)
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
            if (taskReceive != null)
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
    }
}
