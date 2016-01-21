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
            Assert.IsTrue(CreateDummyColorState1() == CreateDummyColorState1());
            Assert.IsTrue(CreateDummyColorState1().Equals(CreateDummyColorState1()));
            Assert.IsTrue(CreateDummyColorState2() == CreateDummyColorState2());
            Assert.IsTrue(CreateDummyColorState2().Equals(CreateDummyColorState2()));
            Assert.IsFalse(CreateDummyColorState1() == CreateDummyColorState1a());
            Assert.IsFalse(CreateDummyColorState1().Equals(CreateDummyColorState1a()));
            Assert.IsFalse(CreateDummyColorState1() == CreateDummyColorState1b());
            Assert.IsFalse(CreateDummyColorState1().Equals(CreateDummyColorState1b()));
            Assert.IsFalse(CreateDummyColorState1() == CreateDummyColorState1c());
            Assert.IsFalse(CreateDummyColorState1().Equals(CreateDummyColorState1c()));
        }

        [TestMethod]
        public void 各プロパティの値はコンストラクタのパラメータに等しい()
        {
            // #102030
            var c1 = CreateDummyColorState1();
            Assert.IsTrue(c1.R == 0x10);
            Assert.IsTrue(c1.G == 0x20);
            Assert.IsTrue(c1.B == 0x30);

            // #000000
            var c2 = CreateDummyColorState2();
            Assert.IsTrue(c2.R == 0);
            Assert.IsTrue(c2.G == 0);
            Assert.IsTrue(c2.B == 0);

            // #ffffff
            var c3 = CreateDummyColorState3();
            Assert.IsTrue(c3.R == 255);
            Assert.IsTrue(c3.G == 255);
            Assert.IsTrue(c3.B == 255);
        }

        /// <summary>
        /// #102030
        /// </summary>
        /// <returns></returns>
        public static ColorState CreateDummyColorState1()
        {
            return new ColorState(0x10, 0x20, 0x30);
        }

        /// <summary>
        /// (1とRが異なる)
        /// #112030
        /// </summary>
        /// <returns></returns>
        public static ColorState CreateDummyColorState1a()
        {
            return new ColorState(0x11, 0x20, 0x30);
        }

        /// <summary>
        /// (1とGが異なる)
        /// #102230
        /// </summary>
        /// <returns></returns>
        public static ColorState CreateDummyColorState1b()
        {
            return new ColorState(0x10, 0x22, 0x30);
        }

        /// <summary>
        /// (1とBが異なる)
        /// #102033
        /// </summary>
        /// <returns></returns>
        public static ColorState CreateDummyColorState1c()
        {
            return new ColorState(0x10, 0x20, 0x33);
        }

        /// <summary>
        /// #000000
        /// </summary>
        /// <returns></returns>
        public static ColorState CreateDummyColorState2()
        {
            return new ColorState(0, 0, 0);
        }

        /// <summary>
        /// #ffffff
        /// </summary>
        /// <returns></returns>
        public static ColorState CreateDummyColorState3()
        {
            return new ColorState(255, 255, 255);
        }
    }
}
