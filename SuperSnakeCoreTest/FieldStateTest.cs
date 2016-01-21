using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperSnake.Core;
using System;
using System.Collections.Generic;

namespace SuperSnakeCoreTest
{
    [TestClass]
    public class FieldStateTest
    {
        [TestMethod]
        public void 同じパラメータで生成されたFieldStateは等しい()
        {
            Assert.IsTrue(CreateDummyFieldState1() == CreateDummyFieldState1());
            Assert.IsTrue(CreateDummyFieldState1().Equals(CreateDummyFieldState1()));
            Assert.IsTrue(CreateDummyFieldState2() == CreateDummyFieldState2());
            Assert.IsTrue(CreateDummyFieldState2().Equals(CreateDummyFieldState2()));
            Assert.IsTrue(CreateDummyFieldState3() == CreateDummyFieldState3());
            Assert.IsTrue(CreateDummyFieldState3().Equals(CreateDummyFieldState3()));
            Assert.IsFalse(CreateDummyFieldState1() == CreateDummyFieldState1a());
            Assert.IsFalse(CreateDummyFieldState1().Equals(CreateDummyFieldState1a()));
            Assert.IsFalse(CreateDummyFieldState1() == CreateDummyFieldState1b());
            Assert.IsFalse(CreateDummyFieldState1().Equals(CreateDummyFieldState1b()));
            Assert.IsFalse(CreateDummyFieldState1() == CreateDummyFieldState1c());
            Assert.IsFalse(CreateDummyFieldState1().Equals(CreateDummyFieldState1c()));
            Assert.IsFalse(CreateDummyFieldState1() == CreateDummyFieldState1d());
            Assert.IsFalse(CreateDummyFieldState1().Equals(CreateDummyFieldState1d()));
        }

        [TestMethod]
        public void 各プロパティの値はコンストラクタのパラメータに等しい()
        {
            // "field1", 2x2
            // xo #ff0000 #000000
            // ox #000000 #0000ff
            var f1 = CreateDummyFieldState1();
            Assert.IsTrue(f1.Name == "field1");
            Assert.IsTrue(f1.Width == 2);
            Assert.IsTrue(f1.Height == 2);
            Assert.IsTrue(f1.Cells[0][0] == new CellState(new ColorState(255, 0, 0), false));
            Assert.IsTrue(f1.Cells[0][1] == new CellState(new ColorState(0, 0, 0), true));
            Assert.IsTrue(f1.Cells[1][0] == new CellState(new ColorState(0, 0, 0), true));
            Assert.IsTrue(f1.Cells[1][1] == new CellState(new ColorState(0, 0, 255), false));

            // "field2", 1x2
            // x #ff0000
            // o #000000
            var f2 = CreateDummyFieldState2();
            Assert.IsTrue(f2.Name == "field2");
            Assert.IsTrue(f2.Width == 1);
            Assert.IsTrue(f2.Height == 2);
            Assert.IsTrue(f2.Cells[0][0] == new CellState(new ColorState(255, 0, 0), false));
            Assert.IsTrue(f2.Cells[0][1] == new CellState(new ColorState(0, 0, 0), true));

            // "field3", 2x1
            // ox #0000ff #00ff00
            var f3 = CreateDummyFieldState3();
            Assert.IsTrue(f3.Name == "field3");
            Assert.IsTrue(f3.Width == 2);
            Assert.IsTrue(f3.Height == 1);
            Assert.IsTrue(f3.Cells[0][0] == new CellState(new ColorState(0, 0, 255), true));
            Assert.IsTrue(f3.Cells[1][0] == new CellState(new ColorState(0, 255, 0), false));

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
            var f4 = CreateDummyFieldState4();
            Assert.IsTrue(f4.Name == "field4");
            Assert.IsTrue(f4.Width == 12);
            Assert.IsTrue(f4.Height == 12);
            for (var x = 0; x < 12; ++x)
            {
                for (var y = 0; y < 12; ++y)
                {
                    Assert.IsTrue(f4.Cells[x][y] == new CellState(new ColorState(255, 255, 255), true));
                }
            }
        }

        /// <summary>
        /// "field1", 2x2
        /// xo #ff0000 #000000
        /// ox #000000 #0000ff
        /// </summary>
        /// <returns></returns>
        public static FieldState CreateDummyFieldState1()
        {
            return new FieldState(
                "field1", new List<IList<CellState>>
                {
                    new List<CellState>
                    {
                        new CellState(new ColorState(255, 0, 0), false),
                        new CellState(new ColorState(0, 0, 0), true),
                    },
                    new List<CellState>
                    {
                        new CellState(new ColorState(0, 0, 0), true),
                        new CellState(new ColorState(0, 0, 255), false),
                    },
                });
        }

        /// <summary>
        /// ("field1"と名前が異なる)
        /// "field1a", 2x2
        /// xo #ff0000 #000000
        /// ox #000000 #0000ff
        /// </summary>
        /// <returns></returns>
        public static FieldState CreateDummyFieldState1a()
        {
            return new FieldState(
                "field1a", new List<IList<CellState>>
                {
                    new List<CellState>
                    {
                        new CellState(new ColorState(255, 0, 0), false),
                        new CellState(new ColorState(0, 0, 0), true),
                    },
                    new List<CellState>
                    {
                        new CellState(new ColorState(0, 0, 0), true),
                        new CellState(new ColorState(0, 0, 255), false),
                    },
                });
        }

        /// <summary>
        /// ("field1"とセル(1, 1)が異なる)
        /// "field1", 2x2
        /// xo #ff0000 #000000
        /// oo #000000 #0000ff
        /// </summary>
        /// <returns></returns>
        public static FieldState CreateDummyFieldState1b()
        {
            return new FieldState(
                "field1", new List<IList<CellState>>
                {
                    new List<CellState>
                    {
                        new CellState(new ColorState(255, 0, 0), false),
                        new CellState(new ColorState(0, 0, 0), true),
                    },
                    new List<CellState>
                    {
                        new CellState(new ColorState(0, 0, 0), true),
                        new CellState(new ColorState(0, 0, 255), true),
                    },
                });
        }

        /// <summary>
        /// ("field1"と幅が異なる)
        /// "field1", 1x2
        /// x #ff0000
        /// o #000000
        /// </summary>
        /// <returns></returns>
        public static FieldState CreateDummyFieldState1c()
        {
            return new FieldState(
                "field1", new List<IList<CellState>>
                {
                    new List<CellState>
                    {
                        new CellState(new ColorState(255, 0, 0), false),
                        new CellState(new ColorState(0, 0, 0), true),
                    },
                });
        }

        /// <summary>
        /// ("field1"と高さが異なる)
        /// "field1", 2x1
        /// xo #ff0000 #000000
        /// </summary>
        /// <returns></returns>
        public static FieldState CreateDummyFieldState1d()
        {
            return new FieldState(
                "field1", new List<IList<CellState>>
                {
                    new List<CellState>
                    {
                        new CellState(new ColorState(255, 0, 0), false),
                    },
                    new List<CellState>
                    {
                        new CellState(new ColorState(0, 0, 0), true),
                    },
                });
        }

        /// <summary>
        /// "field2", 1x2
        /// x #ff0000
        /// o #000000
        /// </summary>
        /// <returns></returns>
        public static FieldState CreateDummyFieldState2()
        {
            return new FieldState(
                "field2", new List<IList<CellState>>
                {
                    new List<CellState>
                    {
                        new CellState(new ColorState(255, 0, 0), false),
                        new CellState(new ColorState(0, 0, 0), true)
                    },
                });
        }

        /// <summary>
        /// "field3", 2x1
        /// ox #0000ff #00ff00
        /// </summary>
        /// <returns></returns>
        public static FieldState CreateDummyFieldState3()
        {
            return new FieldState(
                "field3", new List<IList<CellState>>
                {
                    new List<CellState>
                    {
                        new CellState(new ColorState(0, 0, 255), true)
                    },
                    new List<CellState>
                    {
                        new CellState(new ColorState(0, 255, 0), false)
                    },
                });
        }

        /// <summary>
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
        /// </summary>
        /// <returns></returns>
        public static FieldState CreateDummyFieldState4()
        {
            var cells = new List<IList<CellState>>();
            for (var x = 0; x < 12; ++x)
            {
                var row = new List<CellState>();
                for (var y = 0; y < 12; ++y)
                {
                    row.Add(new CellState(new ColorState(255, 255, 255), true));
                }
                cells.Add(row);
            }
            return new FieldState("field4", cells);
        }
    }
}
