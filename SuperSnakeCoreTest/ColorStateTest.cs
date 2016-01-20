using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSnake.Core;
using System;

namespace SuperSnakeCoreTest
{
    [TestClass]
    public class ColorStateTest
    {
        [TestMethod]
        public void 同じパラメータで生成されたColorStateは等しい()
        {
            Assert.IsTrue(new ColorState(1, 2, 3) == new ColorState(1, 2, 3));
            Assert.IsTrue(new ColorState(0, 0, 0) == new ColorState(0, 0, 0));
            Assert.IsTrue(new ColorState(255, 255, 255) == new ColorState(255, 255, 255));
            Assert.IsFalse(new ColorState(12, 34, 56) == new ColorState(21, 34, 56));
            Assert.IsFalse(new ColorState(14, 25, 36) == new ColorState(14, 52, 36));
            Assert.IsFalse(new ColorState(112, 134, 156) == new ColorState(112, 134, 165));
        }

        [TestMethod]
        public void 各プロパティの値はコンストラクタのパラメータに等しい()
        {
            var c1 = new ColorState(10, 20, 30);
            Assert.IsTrue(c1.R == 10);
            Assert.IsTrue(c1.G == 20);
            Assert.IsTrue(c1.B == 30);

            var c2 = new ColorState(89, 144, 233);
            Assert.IsTrue(c2.R == 89);
            Assert.IsTrue(c2.G == 144);
            Assert.IsTrue(c2.B == 233);
        }
    }
}
