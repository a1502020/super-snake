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
            Assert.IsTrue(CreateRight() == CreateRight());
            Assert.IsTrue(CreateRight().Equals(CreateRight()));
            Assert.IsTrue(CreateRightUp() == CreateRightUp());
            Assert.IsTrue(CreateRightUp().Equals(CreateRightUp()));
            Assert.IsTrue(CreateUp() == CreateUp());
            Assert.IsTrue(CreateUp().Equals(CreateUp()));
            Assert.IsTrue(CreateLeftUp() == CreateLeftUp());
            Assert.IsTrue(CreateLeftUp().Equals(CreateLeftUp()));
            Assert.IsTrue(CreateLeft() == CreateLeft());
            Assert.IsTrue(CreateLeft().Equals(CreateLeft()));
            Assert.IsTrue(CraeteLeftDown() == CraeteLeftDown());
            Assert.IsTrue(CraeteLeftDown().Equals(CraeteLeftDown()));
            Assert.IsTrue(CreateDown() == CreateDown());
            Assert.IsTrue(CreateDown().Equals(CreateDown()));
            Assert.IsTrue(CreateRightDown() == CreateRightDown());
            Assert.IsTrue(CreateRightDown().Equals(CreateRightDown()));
            Assert.IsFalse(CreateRight() == CreateRightUp());
            Assert.IsFalse(CreateRight().Equals(CreateRightUp()));
            Assert.IsFalse(CreateRight() == CreateRightDown());
            Assert.IsFalse(CreateRight().Equals(CreateRightDown()));
            Assert.IsFalse(CreateLeft() == CreateLeftUp());
            Assert.IsFalse(CreateLeft().Equals(CreateLeftUp()));
            Assert.IsFalse(CreateLeft() == CraeteLeftDown());
            Assert.IsFalse(CreateLeft().Equals(CraeteLeftDown()));
            Assert.IsFalse(CreateUp() == CreateDown());
            Assert.IsFalse(CreateUp().Equals(CreateDown()));
        }

        [TestMethod]
        public void 各プロパティの値はコンストラクタのパラメータに等しい()
        {
            Assert.IsTrue(CreateRight().Value == Direction.Right);
            Assert.IsTrue(CreateRightUp().Value == Direction.RightUp);
            Assert.IsTrue(CreateUp().Value == Direction.Up);
            Assert.IsTrue(CreateLeftUp().Value == Direction.LeftUp);
            Assert.IsTrue(CreateLeft().Value == Direction.Left);
            Assert.IsTrue(CraeteLeftDown().Value == Direction.LeftDown);
            Assert.IsTrue(CreateDown().Value == Direction.Down);
            Assert.IsTrue(CreateRightDown().Value == Direction.RightDown);
        }

        public static DirectionState CreateRight()
        {
            return new DirectionState(Direction.Right);
        }

        public static DirectionState CreateRightUp()
        {
            return new DirectionState(Direction.RightUp);
        }

        public static DirectionState CreateUp()
        {
            return new DirectionState(Direction.Up);
        }

        public static DirectionState CreateLeftUp()
        {
            return new DirectionState(Direction.LeftUp);
        }

        public static DirectionState CreateLeft()
        {
            return new DirectionState(Direction.Left);
        }

        public static DirectionState CraeteLeftDown()
        {
            return new DirectionState(Direction.LeftDown);
        }

        public static DirectionState CreateDown()
        {
            return new DirectionState(Direction.Down);
        }

        public static DirectionState CreateRightDown()
        {
            return new DirectionState(Direction.RightDown);
        }
    }
}
