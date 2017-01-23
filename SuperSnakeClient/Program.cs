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

            // 接続先
            Console.Error.Write("IP:port > ");
            var uriText = Console.ReadLine();
            var uri = new Uri("ws://" + uriText);

            // クライアント
            Client client = new Clients.KeyboardClient(DX.KEY_INPUT_DOWN, DX.KEY_INPUT_LEFT, DX.KEY_INPUT_RIGHT);

            // 接続フェーズから開始
            Phase phase = new ConnectionPhase(uri, client);

            // DxLib 初期化
            DX.ChangeWindowMode(DX.TRUE);
            DX.SetDoubleStartValidFlag(DX.TRUE);
            DX.SetAlwaysRunFlag(DX.TRUE);
            DX.SetMainWindowText(string.Format("{0} - Super Snake Client", client.Name));
            DX.DxLib_Init();
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);

            // DxLib メインループ
            while (DX.ProcessMessage() == 0)
            {
                phase = phase.Update();
                if (phase == null)
                {
                    break;
                }
            }

            // DxLib 終了
            DX.DxLib_End();
        }
    }
}
