using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSnake.Core;
using System;

namespace SuperSnakeCoreTest
{
    [TestClass]
    public class PlayerStateTest
    {
        [TestMethod]
        public void 同じパラメータで生成されたPlayerStateは等しい()
        {
            Assert.IsTrue(CreateDummyPlayerState1() == CreateDummyPlayerState1());
            Assert.IsTrue(CreateDummyPlayerState1().Equals(CreateDummyPlayerState1()));
            Assert.IsTrue(CreateDummyPlayerState2() == CreateDummyPlayerState2());
            Assert.IsTrue(CreateDummyPlayerState2().Equals(CreateDummyPlayerState2()));
            Assert.IsFalse(CreateDummyPlayerState1() == CreateDummyPlayerState1a());
            Assert.IsFalse(CreateDummyPlayerState1().Equals(CreateDummyPlayerState1a()));
            Assert.IsFalse(CreateDummyPlayerState1() == CreateDummyPlayerState1b());
            Assert.IsFalse(CreateDummyPlayerState1().Equals(CreateDummyPlayerState1b()));
            Assert.IsFalse(CreateDummyPlayerState1() == CreateDummyPlayerState1c());
            Assert.IsFalse(CreateDummyPlayerState1().Equals(CreateDummyPlayerState1c()));
            Assert.IsFalse(CreateDummyPlayerState1() == CreateDummyPlayerState1d());
            Assert.IsFalse(CreateDummyPlayerState1().Equals(CreateDummyPlayerState1d()));
            Assert.IsFalse(CreateDummyPlayerState1() == CreateDummyPlayerState1e());
            Assert.IsFalse(CreateDummyPlayerState1().Equals(CreateDummyPlayerState1e()));
            Assert.IsFalse(CreateDummyPlayerState1() == CreateDummyPlayerState1f());
            Assert.IsFalse(CreateDummyPlayerState1().Equals(CreateDummyPlayerState1f()));
        }

        [TestMethod]
        public void 各プロパティの値はコンストラクタのパラメータに等しい()
        {
            // 1, "player1", #ff0000
            // (1, 1), Right, Alive
            var p1 = CreateDummyPlayerState1();
            Assert.IsTrue(p1.Number == 1);
            Assert.IsTrue(p1.Name == "player1");
            Assert.IsTrue(p1.Color == new ColorState(255, 0, 0));
            Assert.IsTrue(p1.Position == new PositionState(1, 1));
            Assert.IsTrue(p1.Direction == new DirectionState(Direction.Right));
            Assert.IsTrue(p1.Alive);
            Assert.IsTrue(p1.Dead == !p1.Alive);

            // 2, "player2", #0000ff
            // (10, 10), Left, Dead
            var p2 = CreateDummyPlayerState2();
            Assert.IsTrue(p2.Number == 2);
            Assert.IsTrue(p2.Name == "player2");
            Assert.IsTrue(p2.Color == new ColorState(0, 0, 255));
            Assert.IsTrue(p2.Position == new PositionState(10, 10));
            Assert.IsTrue(p2.Direction == new DirectionState(Direction.Left));
            Assert.IsTrue(!p2.Alive);
            Assert.IsTrue(p2.Dead == !p2.Alive);
        }

        /// <summary>
        /// 1, "player1", #ff0000
        /// (1, 1), Right, Alive
        /// </summary>
        /// <returns></returns>
        public static PlayerState CreateDummyPlayerState1()
        {
            return new PlayerState(
                1, "player1", new ColorState(255, 0, 0),
                new PositionState(1, 1), new DirectionState(Direction.Right),
                true
                );
        }

        /// <summary>
        /// ("player1"とプレイヤー番号が異なる)
        /// 3, "player1", #ff0000
        /// (1, 1), Right, Alive
        /// </summary>
        /// <returns></returns>
        public static PlayerState CreateDummyPlayerState1a()
        {
            return new PlayerState(
                3, "player1", new ColorState(255, 0, 0),
                new PositionState(1, 1), new DirectionState(Direction.Right),
                true
                );
        }

        /// <summary>
        /// ("player1"とプレイヤー名が異なる)
        /// 1, "player3", #ff0000
        /// (1, 1), Right, Alive
        /// </summary>
        /// <returns></returns>
        public static PlayerState CreateDummyPlayerState1b()
        {
            return new PlayerState(
                1, "player3", new ColorState(255, 0, 0),
                new PositionState(1, 1), new DirectionState(Direction.Right),
                true
                );
        }

        /// <summary>
        /// ("player1"と色が異なる)
        /// 1, "player1", #ffff00
        /// (1, 1), Right, Alive
        /// </summary>
        /// <returns></returns>
        public static PlayerState CreateDummyPlayerState1c()
        {
            return new PlayerState(
                1, "player1", new ColorState(255, 255, 0),
                new PositionState(1, 1), new DirectionState(Direction.Right),
                true
                );
        }

        /// <summary>
        /// ("player1"と位置が異なる)
        /// 1, "player1", #ff0000
        /// (10, 1), Right, Alive
        /// </summary>
        /// <returns></returns>
        public static PlayerState CreateDummyPlayerState1d()
        {
            return new PlayerState(
                1, "player1", new ColorState(255, 0, 0),
                new PositionState(10, 1), new DirectionState(Direction.Right),
                true
                );
        }

        /// <summary>
        /// ("player1"と向きが異なる
        /// 1, "player1", #ff0000
        /// (1, 1), Down, Alive
        /// </summary>
        /// <returns></returns>
        public static PlayerState CreateDummyPlayerState1e()
        {
            return new PlayerState(
                1, "player1", new ColorState(255, 0, 0),
                new PositionState(1, 1), new DirectionState(Direction.Down),
                true
                );
        }

        /// <summary>
        /// ("player1"と生死が異なる)
        /// 1, "player1", #ff0000
        /// (1, 1), Right, Dead
        /// </summary>
        /// <returns></returns>
        public static PlayerState CreateDummyPlayerState1f()
        {
            return new PlayerState(
                1, "player1", new ColorState(255, 0, 0),
                new PositionState(1, 1), new DirectionState(Direction.Right),
                false
                );
        }

        /// <summary>
        /// 2, "player2", #0000ff
        /// (10, 10), Left, Dead
        /// </summary>
        /// <returns></returns>
        public static PlayerState CreateDummyPlayerState2()
        {
            return new PlayerState(
                2, "player2", new ColorState(0, 0, 255),
                new PositionState(10, 10), new DirectionState(Direction.Left),
                false
                );
        }
    }
}
