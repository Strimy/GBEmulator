using System;
using System.Linq.Expressions;
using Emulator.GB.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emulator.GB.Core.Tests.Bits
{
    [TestClass]
    public class TestBitL : TestBit
    {
        protected override Expression<Func<ICpu, byte>> GetRegisterExpression => c => c.L;

        [TestMethod]
        public void TestBit0()
        {
            RunBitTest(0, 0x45);
        }

        [TestMethod]
        public void TestBit1()
        {
            RunBitTest(1, 0x4D);

        }

        [TestMethod]
        public void TestBit2()
        {
            RunBitTest(2, 0x55);

        }
        [TestMethod]
        public void TestBit3()
        {
            RunBitTest(3, 0x5D);

        }

        [TestMethod]
        public void TestBit4()
        {
            RunBitTest(4, 0x65);

        }

        [TestMethod]
        public void TestBit5()
        {
            RunBitTest(5, 0x6D);

        }
        [TestMethod]
        public void TestBit6()
        {
            RunBitTest(6, 0x75);

        }

        [TestMethod]
        public void TestBit7()
        {
            RunBitTest(7, 0x7D);

        }

    }
}
