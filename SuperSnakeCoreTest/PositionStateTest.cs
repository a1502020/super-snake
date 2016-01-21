using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSnake.Core;
using System;

namespace SuperSnakeCoreTest
{
    [TestClass]
    public class PositionStateTest
    {
        [TestMethod]
        public void 同じパラメータで生成されたPositionStateは等しい()
        {
            Assert.IsTrue(new PositionState(0, 0) == new PositionState(0, 0));
            Assert.IsTrue(new PositionState(0, 0).Equals(new PositionState(0, 0)));
            Assert.IsTrue(new PositionState(123, 456) == new PositionState(123, 456));
            Assert.IsTrue(new PositionState(123, 456).Equals(new PositionState(123, 456)));
            Assert.IsTrue(new PositionState(-10, -20) == new PositionState(-10, -20));
            Assert.IsTrue(new PositionState(-10, -20).Equals(new PositionState(-10, -20)));
            Assert.IsFalse(new PositionState(1, 2) == new PositionState(0, 2));
            Assert.IsFalse(new PositionState(1, 2).Equals(new PositionState(0, 2)));
            Assert.IsFalse(new PositionState(3, -4) == new PositionState(3, 4));
            Assert.IsFalse(new PositionState(3, -4).Equals(new PositionState(3, 4)));
        }

        [TestMethod]
        public void 各プロパティの値はコンストラクタのパラメータに等しい()
        {
            var p1 = new PositionState(12, 34);
            Assert.IsTrue(p1.X == 12);
            Assert.IsTrue(p1.Y == 34);

            var p2 = new PositionState(-1, -2);
            Assert.IsTrue(p2.X == -1);
            Assert.IsTrue(p2.Y == -2);
        }
    }
}
