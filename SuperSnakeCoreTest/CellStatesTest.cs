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
            Assert.IsTrue(CreateDummyCellState1() == CreateDummyCellState1());
            Assert.IsTrue(CreateDummyCellState1().Equals(CreateDummyCellState1()));
            Assert.IsTrue(CreateDummyCellState2() == CreateDummyCellState2());
            Assert.IsTrue(CreateDummyCellState2().Equals(CreateDummyCellState2()));
            Assert.IsFalse(CreateDummyCellState1() == CreateDummyCellState1a());
            Assert.IsFalse(CreateDummyCellState1().Equals(CreateDummyCellState1a()));
            Assert.IsFalse(CreateDummyCellState1() == CreateDummyCellState1b());
            Assert.IsFalse(CreateDummyCellState1().Equals(CreateDummyCellState1b()));
        }

        [TestMethod]
        public void 各プロパティの値はコンストラクタのパラメータに等しい()
        {
            // o(#000102)
            var c1 = CreateDummyCellState1();
            Assert.IsTrue(c1.Color == new ColorState(0, 1, 2));
            Assert.IsTrue(c1.Passable);

            // x(#1080ff)
            var c2 = CreateDummyCellState2();
            Assert.IsTrue(c2.Color == new ColorState(0x10, 0x80, 0xff));
            Assert.IsTrue(!c2.Passable);
        }

        /// <summary>
        /// o(#000102)
        /// </summary>
        /// <returns></returns>
        public static CellState CreateDummyCellState1()
        {
            return new CellState(new ColorState(0, 1, 2), true);
        }

        /// <summary>
        /// (1と色が異なる)
        /// o(#000103)
        /// </summary>
        /// <returns></returns>
        public static CellState CreateDummyCellState1a()
        {
            return new CellState(new ColorState(0, 1, 3), true);
        }

        /// <summary>
        /// (1と通行可能か否かが異なる)
        /// x(#000102)
        /// </summary>
        /// <returns></returns>
        public static CellState CreateDummyCellState1b()
        {
            return new CellState(new ColorState(0, 1, 2), false);
        }

        /// <summary>
        /// x(#1080ff)
        /// </summary>
        /// <returns></returns>
        public static CellState CreateDummyCellState2()
        {
            return new CellState(new ColorState(16, 128, 255), false);
        }
    }
}
