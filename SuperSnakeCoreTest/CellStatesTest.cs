using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSnake.Core;
using System;

namespace SuperSnakeCoreTest
{
    [TestClass]
    public class CellStatesTest
    {
        [TestMethod]
        public void 同じパラメータで生成されたCellStateは等しい()
        {
            Assert.IsTrue(
                new CellState(new ColorState(11, 22, 33), true) ==
                new CellState(new ColorState(11, 22, 33), true)
                );
            Assert.IsTrue(
                new CellState(new ColorState(100, 150, 200), false) ==
                new CellState(new ColorState(100, 150, 200), false)
                );
            Assert.IsFalse(
                new CellState(new ColorState(12, 34, 56), true) ==
                new CellState(new ColorState(12, 43, 56), true)
                );
            Assert.IsFalse(
                new CellState(new ColorState(12, 34, 56), true) ==
                new CellState(new ColorState(12, 34, 56), false)
                );
        }

        [TestMethod]
        public void 各プロパティの値はコンストラクタのパラメータに等しい()
        {
            var c1 = new CellState(new ColorState(10, 20, 30), true);
            Assert.IsTrue(c1.Color == new ColorState(10, 20, 30));
            Assert.IsTrue(c1.Passable);

            var c2 = new CellState(new ColorState(40, 50, 60), false);
            Assert.IsTrue(c2.Color == new ColorState(40, 50, 60));
            Assert.IsTrue(!c2.Passable);
        }
    }
}
