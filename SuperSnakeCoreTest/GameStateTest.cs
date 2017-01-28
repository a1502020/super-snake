using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSnake.Core;
using System;
using System.Collections.Generic;

namespace SuperSnakeCoreTest
{
    [TestClass]
    public class GameStateTest
    {
        [TestMethod]
        public void 同じパラメータで生成されたGameStateは等しい()
        {
            Assert.IsTrue(CreateDummyGameState1() == CreateDummyGameState1());
            Assert.IsTrue(CreateDummyGameState1().Equals(CreateDummyGameState1()));
            Assert.IsTrue(CreateDummyGameState2() == CreateDummyGameState2());
            Assert.IsTrue(CreateDummyGameState2().Equals(CreateDummyGameState2()));
            Assert.IsFalse(CreateDummyGameState1() == CreateDummyGameState1a());
            Assert.IsFalse(CreateDummyGameState1().Equals(CreateDummyGameState1a()));
            Assert.IsFalse(CreateDummyGameState1() == CreateDummyGameState1b());
            Assert.IsFalse(CreateDummyGameState1().Equals(CreateDummyGameState1b()));
            Assert.IsFalse(CreateDummyGameState1() == CreateDummyGameState1c());
            Assert.IsFalse(CreateDummyGameState1().Equals(CreateDummyGameState1c()));
            Assert.IsFalse(CreateDummyGameState1() == CreateDummyGameState1d());
            Assert.IsFalse(CreateDummyGameState1().Equals(CreateDummyGameState1d()));
        }

        [TestMethod]
        public void 各プロパティの値はコンストラクタのパラメータに等しい()
        {
            // turn: 0
            // field:
            // "field1", 2x2
            // xo #ff0000 #000000
            // ox #000000 #0000ff
            // players[0]:
            // 1, "player1", #ff0000
            // (1, 1), Right, Alive
            var g1 = CreateDummyGameState1();
            Assert.IsTrue(g1.Field == FieldStateTest.CreateDummyFieldState1());
            Assert.IsTrue(g1.PlayersCount == 1);
            Assert.IsTrue(g1.Players.Count == g1.PlayersCount);
            Assert.IsTrue(g1.Players[0] == PlayerStateTest.CreateDummyPlayerState1());
            Assert.IsTrue(g1.Turn == 0);
            Assert.IsTrue(g1.Finished);
            Assert.IsTrue(g1.Winner == g1.Players[0]);
            Assert.IsTrue(g1.WinnerPlayerNum == 0);

            // turn: 123
            // field:
            // "field4", 12x12
            // oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
            // oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
            // oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
            // oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
            // oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
            // oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
            // oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
            // oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
            // oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
            // oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
            // oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
            // oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
            // players[0]:
            // 1, "player1", #ff0000
            // (1, 1), Right, Alive
            // players[1]:
            // 2, "player2", #0000ff
            // (10, 10), Left, Dead
            var g2 = CreateDummyGameState2();
            Assert.IsTrue(g2.Field == FieldStateTest.CreateDummyFieldState4());
            Assert.IsTrue(g2.PlayersCount == 2);
            Assert.IsTrue(g2.Players.Count == g2.PlayersCount);
            Assert.IsTrue(g2.Players[0] == PlayerStateTest.CreateDummyPlayerState1());
            Assert.IsTrue(g2.Players[1] == PlayerStateTest.CreateDummyPlayerState2());
            Assert.IsTrue(g2.Turn == 123);
            Assert.IsTrue(g2.Finished);
            Assert.IsTrue(g2.Winner == g2.Players[0]);
            Assert.IsTrue(g2.WinnerPlayerNum == 0);

            // turn: 0
            // field:
            // "field1", 2x2
            // xo #ff0000 #000000
            // ox #000000 #0000ff
            // players[0]:
            // 1, "player1", #ff0000
            // (1, 1), Right, Alive
            // players[1]:
            // 3, "player1", #ff0000
            // (1, 1), Right, Alive
            var g1c = CreateDummyGameState1c();
            Assert.IsTrue(g1c.Field == FieldStateTest.CreateDummyFieldState1());
            Assert.IsTrue(g1c.PlayersCount == 2);
            Assert.IsTrue(g1c.Players.Count == g1c.PlayersCount);
            Assert.IsTrue(g1c.Players[0] == PlayerStateTest.CreateDummyPlayerState1());
            Assert.IsTrue(g1c.Players[1] == PlayerStateTest.CreateDummyPlayerState1a());
            Assert.IsTrue(g1c.Turn == 0);
            Assert.IsFalse(g1c.Finished);
            Assert.IsTrue((object)g1c.Winner == null);
            Assert.IsTrue(g1c.WinnerPlayerNum == -1);
        }

        /// <summary>
        /// turn: 0
        /// field:
        /// "field1", 2x2
        /// xo #ff0000 #000000
        /// ox #000000 #0000ff
        /// players[0]:
        /// 1, "player1", #ff0000
        /// (1, 1), Right, Alive
        /// </summary>
        /// <returns></returns>
        public static GameState CreateDummyGameState1()
        {
            return new GameState(
                FieldStateTest.CreateDummyFieldState1(),
                new List<PlayerState>
                {
                    PlayerStateTest.CreateDummyPlayerState1()
                },
                0
            );
        }

        /// <summary>
        /// ("field1"とフィールドが異なる)
        /// turn: 0
        /// field:
        /// "field1a", 2x2
        /// xo #ff0000 #000000
        /// ox #000000 #0000ff
        /// players[0]:
        /// 1, "player1", #ff0000
        /// (1, 1), Right, Alive
        /// </summary>
        /// <returns></returns>
        public static GameState CreateDummyGameState1a()
        {
            return new GameState(
                FieldStateTest.CreateDummyFieldState1a(),
                new List<PlayerState>
                {
                    PlayerStateTest.CreateDummyPlayerState1()
                },
                0
            );
        }

        /// <summary>
        /// ("field1"とplayers[0]が異なる)
        /// turn: 0
        /// field:
        /// "field1", 2x2
        /// xo #ff0000 #000000
        /// ox #000000 #0000ff
        /// players[0]:
        /// 3, "player1", #ff0000
        /// (1, 1), Right, Alive
        /// </summary>
        /// <returns></returns>
        public static GameState CreateDummyGameState1b()
        {
            return new GameState(
                FieldStateTest.CreateDummyFieldState1(),
                new List<PlayerState>
                {
                    PlayerStateTest.CreateDummyPlayerState1a()
                },
                0
            );
        }

        /// <summary>
        /// ("field1"とプレイヤー数が異なる)
        /// turn: 0
        /// field:
        /// "field1", 2x2
        /// xo #ff0000 #000000
        /// ox #000000 #0000ff
        /// players[0]:
        /// 1, "player1", #ff0000
        /// (1, 1), Right, Alive
        /// players[1]:
        /// 3, "player1", #ff0000
        /// (1, 1), Right, Alive
        /// </summary>
        /// <returns></returns>
        public static GameState CreateDummyGameState1c()
        {
            return new GameState(
                FieldStateTest.CreateDummyFieldState1(),
                new List<PlayerState>
                {
                    PlayerStateTest.CreateDummyPlayerState1(),
                    PlayerStateTest.CreateDummyPlayerState1a(),
                },
                0
            );
        }

        /// <summary>
        /// ("field1"とターン数が異なる)
        /// turn: 1
        /// field:
        /// "field1", 2x2
        /// xo #ff0000 #000000
        /// ox #000000 #0000ff
        /// players[0]:
        /// 1, "player1", #ff0000
        /// (1, 1), Right, Alive
        /// </summary>
        /// <returns></returns>
        public static GameState CreateDummyGameState1d()
        {
            return new GameState(
                FieldStateTest.CreateDummyFieldState1(),
                new List<PlayerState>
                {
                    PlayerStateTest.CreateDummyPlayerState1()
                },
                1
            );
        }

        /// <summary>
        /// turn: 123
        /// field:
        /// "field4", 12x12
        /// oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
        /// oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
        /// oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
        /// oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
        /// oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
        /// oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
        /// oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
        /// oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
        /// oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
        /// oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
        /// oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
        /// oooooooooooo #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff #ffffff
        /// players[0]:
        /// 1, "player1", #ff0000
        /// (1, 1), Right, Alive
        /// players[1]:
        /// 2, "player2", #0000ff
        /// (10, 10), Left, Dead
        /// </summary>
        /// <returns></returns>
        public static GameState CreateDummyGameState2()
        {
            return new GameState(
                FieldStateTest.CreateDummyFieldState4(),
                new List<PlayerState>
                {
                    PlayerStateTest.CreateDummyPlayerState1(),
                    PlayerStateTest.CreateDummyPlayerState2(),
                },
                123
            );
        }
    }
}
