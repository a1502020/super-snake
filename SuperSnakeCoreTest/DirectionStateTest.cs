using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSnake.Core;
using System;

namespace SuperSnakeCoreTest
{
    [TestClass]
    public class DirectionStateTest
    {
        [TestMethod]
        public void 同じパラメータで生成されたDirectionStateは等しい()
        {
            Assert.IsTrue(
                new DirectionState(Direction.Right) ==
                new DirectionState(Direction.Right)
                );
            Assert.IsTrue(
                new DirectionState(Direction.RightUp) ==
                new DirectionState(Direction.RightUp)
                );
            Assert.IsTrue(
                new DirectionState(Direction.Up) ==
                new DirectionState(Direction.Up)
                );
            Assert.IsTrue(
                new DirectionState(Direction.LeftUp) ==
                new DirectionState(Direction.LeftUp)
                );
            Assert.IsTrue(
                new DirectionState(Direction.Left) ==
                new DirectionState(Direction.Left)
                );
            Assert.IsTrue(
                new DirectionState(Direction.LeftDown) ==
                new DirectionState(Direction.LeftDown)
                );
            Assert.IsTrue(
                new DirectionState(Direction.Down) ==
                new DirectionState(Direction.Down)
                );
            Assert.IsTrue(
                new DirectionState(Direction.RightDown) ==
                new DirectionState(Direction.RightDown)
                );
            Assert.IsFalse(
                new DirectionState(Direction.Right) ==
                new DirectionState(Direction.RightUp)
                );
            Assert.IsFalse(
                new DirectionState(Direction.Right) ==
                new DirectionState(Direction.RightDown)
                );
            Assert.IsFalse(
                new DirectionState(Direction.Left) ==
                new DirectionState(Direction.LeftUp)
                );
            Assert.IsFalse(
                new DirectionState(Direction.Left) ==
                new DirectionState(Direction.LeftDown)
                );
            Assert.IsFalse(
                new DirectionState(Direction.Up) ==
                new DirectionState(Direction.Down)
                );
        }

        [TestMethod]
        public void 各プロパティの値はコンストラクタのパラメータに等しい()
        {
            var d1 = new DirectionState(Direction.Right);
            Assert.IsTrue(d1.Value == Direction.Right);
            var d2 = new DirectionState(Direction.RightUp);
            Assert.IsTrue(d2.Value == Direction.RightUp);
            var d3 = new DirectionState(Direction.Up);
            Assert.IsTrue(d3.Value == Direction.Up);
            var d4 = new DirectionState(Direction.LeftUp);
            Assert.IsTrue(d4.Value == Direction.LeftUp);
            var d5 = new DirectionState(Direction.Left);
            Assert.IsTrue(d5.Value == Direction.Left);
            var d6 = new DirectionState(Direction.LeftDown);
            Assert.IsTrue(d6.Value == Direction.LeftDown);
            var d7 = new DirectionState(Direction.Down);
            Assert.IsTrue(d7.Value == Direction.Down);
            var d8 = new DirectionState(Direction.RightDown);
            Assert.IsTrue(d8.Value == Direction.RightDown);
        }
    }
}
