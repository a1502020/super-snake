using SuperSnake.Core;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnakeServer
{
    public class SessionInfo
    {
        public SessionInfo(WebSocketSession s)
        {
            this.Session = s;
            this.TaskSendGameState = null;
        }

        /// <summary>
        /// セッション
        /// </summary>
        public WebSocketSession Session { get; set; }

        /// <summary>
        /// プレイヤー名
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// プレイヤーの色
        /// </summary>
        public ColorState PlayerColor { get; set; }

        /// <summary>
        /// ゲームの状態送信タスク
        /// </summary>
        public Task TaskSendGameState { get; set; }

        public async Task Send(string message)
        {
            await Task.Run(() => this.Session.Send(message));
        }
    }
}
