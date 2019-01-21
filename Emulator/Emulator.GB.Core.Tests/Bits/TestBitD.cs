using System;
using System.Linq.Expressions;
using Emulator.GB.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emulator.GB.Core.Tests.Bits
{
    [TestClass]
    public class TestBitD : TestBit
    {
        protected override Expression<Func<ICpu, byte>> GetRegisterExpression => c => c.D;

        [TestMethod]
        public void TestBit0()
        {
            RunBitTest(0, 0x42);
        }

        [TestMethod]
        public void TestBit1()
        {
            RunBitTest(1, 0x4A);

        }

        [TestMethod]
        public void TestBit2()
        {
            RunBitTest(2, 0x52);

        }
        [TestMethod]
        public void TestBit3()
        {
            RunBitTest(3, 0x5A);

        }

        [TestMethod]
        public void TestBit4()
        {
            RunBitTest(4, 0x62);

        }

        [TestMethod]
        public void TestBit5()
        {
            RunBitTest(5, 0x6A);

        }
        [TestMethod]
        public void TestBit6()
        {
            RunBitTest(6, 0x72);

        }

        [TestMethod]
        public void TestBit7()
        {
            RunBitTest(7, 0x7A);

        }

    }
}
