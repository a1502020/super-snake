using DxLibDLL;
using SuperSnake.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuperSnakeClient
{
    public class ConnectionPhase : Phase
    {
        public ConnectionPhase(Uri uri, Client client)
        {
            this.uri = uri;
            this.client = client;

            taskConnect = ws.ConnectAsync(this.uri, CancellationToken.None);
        }

        private Uri uri;
        private ClientWebSocket ws = new ClientWebSocket();
        private Task taskConnect = null;
        private Client client;
        private Task taskSend = null;

        private int cnt = 0;

        protected override Phase update()
        {
            // 接続したらエントリー情報を送信
            if (taskConnect != null && taskConnect.IsCompleted)
            {
                taskConnect.Dispose();
                taskConnect = null;

                var buff = new ArraySegment<byte>(Encoding.UTF8.GetBytes(client.Name));
                taskSend = ws.SendAsync(buff, WebSocketMessageType.Text, true, CancellationToken.None);
            }

            // エントリー情報の送信が完了したらゲームフェーズへ移行
            if (taskSend != null && taskSend.IsCompleted)
            {
                taskSend.Dispose();
                taskSend = null;

                return new GamePhase(ws, client);
            }

            // 描画
            DX.DrawFillBox(0, 0, 640, 480, DX.GetColor(0, 0, 0));
            var dots = new string('.', cnt / 30);
            if (taskConnect != null)
            {
                DX.DrawString(2, 2,
                    string.Format("{0} に接続中{1}", uri.OriginalString, dots),
                    DX.GetColor(255, 255, 255));
            }
            else if (taskSend != null)
            {
                DX.DrawString(2, 2,
                    string.Format("エントリー情報送信中{0}", dots),
                    DX.GetColor(255, 255, 255));
            }

            ++cnt;
            if (cnt >= 120) cnt = 0;

            return this;
        }
    }
}
