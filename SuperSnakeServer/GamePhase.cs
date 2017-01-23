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

            server.NewMessageReceived += serverNewMessageReceived;

            gameStateDrawer.BasePos = new Position(2, 22);

            // ゲームの状態等を送信
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

        protected override Phase update()
        {
            // 送信が完了したら行動の受信を開始
            for (int playerNum = 0; playerNum < playersCount; playerNum++)
            {
                if (tasksSend[playerNum] != null && tasksSend[playerNum].IsCompleted)
                {
                    tasksSend[playerNum].Dispose();
                    tasksSend[playerNum] = null;

                    if (game.State.Players[playerNum].Alive)
                    {
                        receiving[playerNum] = true;
                    }
                }
            }

            // すべての生きているプレイヤーの行動を受信したらステップ処理を行う
            if (Enumerable.Range(0, playersCount)
                .All(playerNum => game.State.Players[playerNum].Dead || received[playerNum]))
            {
                game.Step(actions);

                // ゲームが終了したら結果フェーズへ
                if (game.State.Players.Count(player => player.Alive) <= 1)
                {
                    beginSend(finished: true);
                    return new ResultPhase(game);
                }

                for (int playerNum = 0; playerNum < playersCount; playerNum++)
                {
                    received[playerNum] = false;
                }

                // ゲームの状態等を送信
                beginSend();
            }

            // 描画
            DX.DrawFillBox(0, 0, 640, 480, DX.GetColor(0, 0, 0));

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

        private void beginSend(bool finished = false)
        {
            for (int playerNum = 0; playerNum < playersCount; playerNum++)
            {
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
                var s = sessions[playerNum].Session;
                tasksSend[playerNum] = Task.Run(() => s.Send(sb.ToString()));
            }
        }
    }
}
