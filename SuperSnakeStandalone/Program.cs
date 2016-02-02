using DxLibDLL;
using SuperSnake.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = SuperSnake.Core.Action;

namespace SuperSnakeStandalone
{
    public class Program
    {
        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        public void Run(string[] args)
        {
            // DxLib 初期化
            DX.ChangeWindowMode(DX.TRUE);
            DX.DxLib_Init();
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);

            drawer = new GameStateDrawer();
            var rnd = new Random();

            // ゲームの初期状態
            var igsReader = new InitialGameStateReader();
            var initState = igsReader.Read("fields/test.txt");
            var playerInfos = new List<PlayerInfo>
            {
                new PlayerInfo("プレイヤー1", new ColorState(255, 0, 0)),
                new PlayerInfo("プレイヤー2", new ColorState(0, 0, 255)),
                new PlayerInfo("プレイヤー3", new ColorState(255, 128, 0)),
                new PlayerInfo("プレイヤー4", new ColorState(0, 128, 0)),
            };
            game = new Game(igsReader.Combine(initState, playerInfos));

            // クライアント
            var clients = new List<Client>();
            for (var i = 0; i < game.State.PlayersCount; ++i)
            {
                clients.Add(null);
            }
            Parallel.For(0, game.State.PlayersCount, i =>
            {
                clients[i] = new Clients.RansuchanClient(rnd);
            });

            // DxLib メインループ
            while (DX.ProcessMessage() == 0)
            {
                // 操作
                key.Update();

                if (key.IsPressed(DX.KEY_INPUT_RETURN))
                {
                    var actions = new List<Action>();
                    for (var i = 0; i < game.State.PlayersCount; ++i)
                    {
                        actions.Add(Action.Straight);
                    }
                    Parallel.For(0, game.State.PlayersCount, i =>
                    {
                        actions[i] = clients[i].Think(game.State, i);
                    });
                    game.Step(actions);
                }

                // 描画
                drawer.Draw(game.State);

                DX.ScreenFlip();
            }

            // DxLib 終了
            DX.DxLib_End();
        }

        private Key key = new Key();
        private Game game;
        private GameStateDrawer drawer;
    }
}
