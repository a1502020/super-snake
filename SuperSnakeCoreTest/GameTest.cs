using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSnake.Core;
using System;
using System.Collections.Generic;

namespace SuperSnakeCoreTest
{
    using Action = SuperSnake.Core.Action;

    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void ステップの移動がルール通り1()
        {
            var g1 = new Game(CreateDummyGameState1());
            g1.Step(new List<Action> { Action.Straight, Action.Straight, Action.Left, Action.Right });
            Assert.IsTrue(g1.State == CreateDummyGameState1a());
        }

        [TestMethod]
        public void 死亡したプレイヤーはステップで移動しない()
        {
            var g1 = new Game(CreateDummyGameState1b());
            g1.Step(new List<Action> { Action.Right, Action.Straight, Action.Left, Action.Right });
            Assert.IsTrue(g1.State == CreateDummyGameState1c());
        }

        [TestMethod]
        public void ステップの死亡判定がルール通り1()
        {
            var g1 = new Game(CreateDummyGameState1a());
            g1.Step(new List<Action> { Action.Right, Action.Left, Action.Left, Action.Left });
            Assert.IsTrue(g1.State == CreateDummyGameState1b());
        }

        [TestMethod]
        public void ステップの死亡判定がルール通り2()
        {
            var g1 = new Game(CreateDummyGameState1c());
            g1.Step(new List<Action> { Action.Left, Action.Straight, Action.Left, Action.Right });
            Assert.IsTrue(g1.State == CreateDummyGameState1d());
        }

        /// <summary>
        /// field:
        /// "field1"
        /// xooox #ffffff #ffffff #ffffff #ffffff #ffffff
        /// ooooo #ffffff #ffffff #ffffff #ffffff #ffffff
        /// ooooo #ffffff #ffffff #ffffff #ffffff #ffffff
        /// ooooo #ffffff #ffffff #ffffff #ffffff #ffffff
        /// xooox #ffffff #ffffff #ffffff #ffffff #ffffff
        /// players[0]:
        /// 1, "Red", #ff0000
        /// (0, 0), RightDown, Alive
        /// players[1]:
        /// 2, "Blue", #0000ff
        /// (4, 4), LeftUp, Alive
        /// players[2]:
        /// 3, "Yellow", #ffff00
        /// (4, 0), LeftDown, Alive
        /// players[3]:
        /// 4, "Green", #00ff00
        /// (0, 4), RightUp, Alive
        /// </summary>
        /// <returns></returns>
        public static GameState CreateDummyGameState1()
        {
            return new GameState(
                new FieldState(
                    "field1",
                    #region cells
                    new List<IList<CellState>>
                    {
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), false),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), false),
                        },
                        new List<CellState>
                        {
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
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), false),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), false),
                        },
                    }
                    #endregion
                ),
                #region players
                new List<PlayerState>
                {
                    new PlayerState(
                        1, "Red", new ColorState(255, 0, 0),
                        new PositionState(0, 0), new DirectionState(Direction.RightDown),
                        true),
                    new PlayerState(
                        2, "Blue", new ColorState(0, 0, 255),
                        new PositionState(4, 4), new DirectionState(Direction.LeftUp),
                        true),
                    new PlayerState(
                        3, "Yellow", new ColorState(255, 255, 0),
                        new PositionState(4, 0), new DirectionState(Direction.LeftDown),
                        true),
                    new PlayerState(
                        4, "Green", new ColorState(0, 255, 0),
                        new PositionState(0, 4), new DirectionState(Direction.RightUp),
                        true),
                }
                #endregion
            );
        }

        /// <summary>
        /// (GameState1からSSLR(1:直進(S)、2:直進、3:左折(L)、4：右折(R)))
        /// field:
        /// "field1"
        /// xooox #ff0000 #ffffff #ffffff #ffffff #ffff00
        /// oxoox #ffffff #ffffff #ffffff #ffffff #ffffff
        /// ooooo #ffffff #ffffff #ffffff #ffffff #ffffff
        /// oooxo #ffffff #ffffff #ffffff #ffffff #ffffff
        /// xxoox #00ff00 #ffffff #ffffff #ffffff #0000ff
        /// players[0]:
        /// 1, "Red", #ff0000
        /// (1, 1), RightDown, Alive
        /// players[1]:
        /// 2, "Blue", #0000ff
        /// (3, 3), LeftUp, Alive
        /// players[2]:
        /// 3, "Yellow", #ffff00
        /// (4, 1), Down, Alive
        /// players[3]:
        /// 4, "Green", #00ff00
        /// (1, 4), Right, Alive
        /// </summary>
        /// <returns></returns>
        public static GameState CreateDummyGameState1a()
        {
            return new GameState(
                new FieldState(
                    "field1",
                    #region cells
                    new List<IList<CellState>>
                    {
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 0, 0), false),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(0, 255, 0), false),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), false),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), false),
                        },
                        new List<CellState>
                        {
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
                            new CellState(new ColorState(255, 255, 255), false),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 0), false),
                            new CellState(new ColorState(255, 255, 255), false),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(0, 0, 255), false),
                        },
                    }
                    #endregion
                ),
                #region players
                new List<PlayerState>
                {
                    new PlayerState(
                        1, "Red", new ColorState(255, 0, 0),
                        new PositionState(1, 1), new DirectionState(Direction.RightDown),
                        true),
                    new PlayerState(
                        2, "Blue", new ColorState(0, 0, 255),
                        new PositionState(3, 3), new DirectionState(Direction.LeftUp),
                        true),
                    new PlayerState(
                        3, "Yellow", new ColorState(255, 255, 0),
                        new PositionState(4, 1), new DirectionState(Direction.Down),
                        true),
                    new PlayerState(
                        4, "Green", new ColorState(0, 255, 0),
                        new PositionState(1, 4), new DirectionState(Direction.Right),
                        true),
                }
                #endregion
            );
        }

        /// <summary>
        /// (GameState1aからRLLL)
        /// field:
        /// "field1"
        /// xooox #ff0000 #ffffff #ffffff #ffffff #ffff00
        /// oxoox #ffffff #ff0000 #ffffff #ffffff #ffff00
        /// oxooo #ffffff #ffffff #ffffff #ffffff #ffffff
        /// ooxxo #ffffff #ffffff #ffffff #0000ff #ffffff
        /// xxoox #00ff00 #00ff00 #ffffff #ffffff #0000ff
        /// players[0]:
        /// 1, "Red", #ff0000
        /// (1, 2), Down, Alive
        /// players[1]:
        /// 2, "Blue", #0000ff
        /// (2, 3), Left, Dead
        /// players[2]:
        /// 3, "Yellow", #ffff00
        /// (5, 2), RightDown, Dead
        /// players[3]:
        /// 4, "Green", #00ff00
        /// (2, 3), RightUp, Dead
        /// </summary>
        /// <returns></returns>
        public static GameState CreateDummyGameState1b()
        {
            return new GameState(
                new FieldState(
                    "field1",
                    #region cells
                    new List<IList<CellState>>
                    {
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 0, 0), false),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(0, 255, 0), false),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 0, 0), false),
                            new CellState(new ColorState(255, 255, 255), false),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(0, 255, 0), false),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), false),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(0, 0, 255), false),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 0), false),
                            new CellState(new ColorState(255, 255, 0), false),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(0, 0, 255), false),
                        },
                    }
                    #endregion
                ),
                #region players
 new List<PlayerState>
                {
                    new PlayerState(
                        1, "Red", new ColorState(255, 0, 0),
                        new PositionState(1, 2), new DirectionState(Direction.Down),
                        true),
                    new PlayerState(
                        2, "Blue", new ColorState(0, 0, 255),
                        new PositionState(2, 3), new DirectionState(Direction.Left),
                        false),
                    new PlayerState(
                        3, "Yellow", new ColorState(255, 255, 0),
                        new PositionState(5, 2), new DirectionState(Direction.RightDown),
                        false),
                    new PlayerState(
                        4, "Green", new ColorState(0, 255, 0),
                        new PositionState(2, 3), new DirectionState(Direction.RightUp),
                        false),
                }
            #endregion
            );
        }

        /// <summary>
        /// (GameState1bからRSLR)
        /// field:
        /// "field1"
        /// xooox #ff0000 #ffffff #ffffff #ffffff #ffff00
        /// oxoox #ffffff #ff0000 #ffffff #ffffff #ffff00
        /// oxooo #ffffff #ff0000 #ffffff #ffffff #ffffff
        /// xoxxo #ffffff #ffffff #ffffff #0000ff #ffffff
        /// xxoox #00ff00 #00ff00 #ffffff #ffffff #0000ff
        /// players[0]:
        /// 1, "Red", #ff0000
        /// (0, 3), LeftDown, Alive
        /// players[1]:
        /// 2, "Blue", #0000ff
        /// (2, 3), Left, Dead
        /// players[2]:
        /// 3, "Yellow", #ffff00
        /// (5, 2), RightDown, Dead
        /// players[3]:
        /// 4, "Green", #00ff00
        /// (2, 3), RightUp, Dead
        /// </summary>
        /// <returns></returns>
        public static GameState CreateDummyGameState1c()
        {
            return new GameState(
                new FieldState(
                    "field1",
                    #region cells
                    new List<IList<CellState>>
                    {
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 0, 0), false),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), false),
                            new CellState(new ColorState(0, 255, 0), false),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 0, 0), false),
                            new CellState(new ColorState(255, 0, 0), false),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(0, 255, 0), false),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), false),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(0, 0, 255), false),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 0), false),
                            new CellState(new ColorState(255, 255, 0), false),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(0, 0, 255), false),
                        },
                    }
                    #endregion
                ),
                #region players
                new List<PlayerState>
                {
                    new PlayerState(
                        1, "Red", new ColorState(255, 0, 0),
                        new PositionState(0, 3), new DirectionState(Direction.LeftDown),
                        true),
                    new PlayerState(
                        2, "Blue", new ColorState(0, 0, 255),
                        new PositionState(2, 3), new DirectionState(Direction.Left),
                        false),
                    new PlayerState(
                        3, "Yellow", new ColorState(255, 255, 0),
                        new PositionState(5, 2), new DirectionState(Direction.RightDown),
                        false),
                    new PlayerState(
                        4, "Green", new ColorState(0, 255, 0),
                        new PositionState(2, 3), new DirectionState(Direction.RightUp),
                        false),
                }
                #endregion
            );
        }

        /// <summary>
        /// (GameState1cからLSLR)
        /// field:
        /// "field1"
        /// xooox #ff0000 #ffffff #ffffff #ffffff #ffff00
        /// oxoox #ffffff #ff0000 #ffffff #ffffff #ffff00
        /// oxooo #ffffff #ff0000 #ffffff #ffffff #ffffff
        /// xoxxo #ff0000 #ffffff #ffffff #0000ff #ffffff
        /// xxoox #00ff00 #00ff00 #ffffff #ffffff #0000ff
        /// players[0]:
        /// 1, "Red", #ff0000
        /// (0, 4), Down, Dead
        /// players[1]:
        /// 2, "Blue", #0000ff
        /// (2, 3), Left, Dead
        /// players[2]:
        /// 3, "Yellow", #ffff00
        /// (5, 2), RightDown, Dead
        /// players[3]:
        /// 4, "Green", #00ff00
        /// (2, 3), RightUp, Dead
        /// </summary>
        /// <returns></returns>
        public static GameState CreateDummyGameState1d()
        {
            return new GameState(
                new FieldState(
                    "field1",
                    #region cells
                    new List<IList<CellState>>
                    {
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 0, 0), false),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 0, 0), false),
                            new CellState(new ColorState(0, 255, 0), false),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 0, 0), false),
                            new CellState(new ColorState(255, 0, 0), false),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(0, 255, 0), false),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), false),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(0, 0, 255), false),
                            new CellState(new ColorState(255, 255, 255), true),
                        },
                        new List<CellState>
                        {
                            new CellState(new ColorState(255, 255, 0), false),
                            new CellState(new ColorState(255, 255, 0), false),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(255, 255, 255), true),
                            new CellState(new ColorState(0, 0, 255), false),
                        },
                    }
                    #endregion
                ),
                #region players
                new List<PlayerState>
                {
                    new PlayerState(
                        1, "Red", new ColorState(255, 0, 0),
                        new PositionState(0, 4), new DirectionState(Direction.Down),
                        false),
                    new PlayerState(
                        2, "Blue", new ColorState(0, 0, 255),
                        new PositionState(2, 3), new DirectionState(Direction.Left),
                        false),
                    new PlayerState(
                        3, "Yellow", new ColorState(255, 255, 0),
                        new PositionState(5, 2), new DirectionState(Direction.RightDown),
                        false),
                    new PlayerState(
                        4, "Green", new ColorState(0, 255, 0),
                        new PositionState(2, 3), new DirectionState(Direction.RightUp),
                        false),
                }
                #endregion
            );
        }
    }
}
