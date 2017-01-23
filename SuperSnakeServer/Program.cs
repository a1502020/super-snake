using DxLibDLL;
using SuperSnake.Core;
using SuperSnake.Util;
using SuperSocket.SocketBase.Config;
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
                MaxConnectionNumber = 4,
                Mode = SuperSocket.SocketBase.SocketMode.Tcp,
                Name = "Super Snake Server",
            };
            Console.Error.Write("Port > ");
            serverConfig.Port = int.Parse(Console.ReadLine());

            // 使用するフィールド
            var igsReader = new InitialGameStateReader();
            var fields = Directory.EnumerateFiles("./fields/")
                .Select(path =>
                {
                    var name = Path.GetFileNameWithoutExtension(path);
                    try
                    {
                        return Tuple.Create(name, igsReader.Read(path));
                    }
                    catch (FormatException)
                    {
                        return Tuple.Create(name, (InitialGameState)null);
                    }
                })
                .Where(p => p.Item2 != null)
                .ToList();
            if (fields.Count == 0)
            {
                Console.Error.WriteLine("フィールドが1つも存在しません。");
                Console.Error.WriteLine("fields ディレクトリにファイルを追加してください。");
                return;
            }
            foreach (var p in fields)
            {
                Console.Error.WriteLine("{0}", p.Item1);
                Console.Error.WriteLine("\t{0,3}x{1,-3}(最大{2,2}人) {3}",
                    p.Item2.Field.Width, p.Item2.Field.Height, p.Item2.PlayersCount, p.Item2.Field.Name);
            }
            Console.Error.Write("使用するフィールド > ");
            var fieldName = Console.ReadLine();
            var initState = fields.Find(p => p.Item1 == fieldName).Item2;
            if (initState == null)
            {
                Console.Error.WriteLine("そのフィールドは存在しません。");
                return;
            }

            serverConfig.MaxConnectionNumber = initState.PlayersCount;

            // サーバー初期化
            server.Setup(rootConfig, serverConfig);

            // DxLib 初期化
            DX.ChangeWindowMode(DX.TRUE);
            DX.SetDoubleStartValidFlag(DX.TRUE);
            DX.SetAlwaysRunFlag(DX.TRUE);
            DX.SetMainWindowText(string.Format("{0} - Super Snake Server", initState.Field.Name));
            DX.DxLib_Init();
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);

            // 接続待ちフェーズから開始
            Phase phase = new ConnectionPhase(server, initState);

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
