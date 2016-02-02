﻿using DxLibDLL;
using SuperSnake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSnakeStandalone
{
    using Action = SuperSnake.Core.Action;

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
            game = new Game(new GameState(testField, testPlayers));

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

        private FieldState testField
        {
            get
            {
                return new FieldState
                (
                    name: "テストフィールド",
                    cells: new List<IList<CellState>>
                    {
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                    }
                );
            }
        }

        private List<PlayerState> testPlayers
        {
            get
            {
                return new List<PlayerState>
                {
                    new PlayerState
                    (
                        number: 0,
                        name: "プレイヤー0",
                        color: new ColorState(255, 0, 0),
                        position: new PositionState(0, 0),
                        direction: new DirectionState(Direction.RightDown),
                        alive: true
                    ),
                    new PlayerState
                    (
                        number: 1,
                        name: "プレイヤー1",
                        color: new ColorState(0, 0, 255),
                        position: new PositionState(11, 11),
                        direction: new DirectionState(Direction.LeftUp),
                        alive: true
                    ),
                    new PlayerState
                    (
                        number: 2,
                        name: "プレイヤー2",
                        color: new ColorState(255, 128, 0),
                        position: new PositionState(11, 0),
                        direction: new DirectionState(Direction.LeftDown),
                        alive: true
                    ),
                    new PlayerState
                    (
                        number: 3,
                        name: "プレイヤー3",
                        color: new ColorState(0, 128, 0),
                        position: new PositionState(0, 11),
                        direction: new DirectionState(Direction.RightUp),
                        alive: true
                    ),
                };
            }
        }
    }
}
