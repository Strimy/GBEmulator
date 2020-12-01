using System;
using System.Linq.Expressions;
using Emulator.GB.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emulator.GB.Core.Tests.Bits
{
    [TestClass]
    public class TestBitE : TestBit
    {
        protected override Expression<Func<ICpu, byte>> GetRegisterExpression => c => c.E;

        [TestMethod]
        public void TestBit0()
        {
            RunBitTest(0, 0x43);
        }

        [TestMethod]
        public void TestBit1()
        {
            RunBitTest(1, 0x4B);

        }

        [TestMethod]
        public void TestBit2()
        {
            RunBitTest(2, 0x53);

        }
        [TestMethod]
        public void TestBit3()
        {
            RunBitTest(3, 0x5B);

        }

        [TestMethod]
        public void TestBit4()
        {
            RunBitTest(4, 0x63);

        }

        [TestMethod]
        public void TestBit5()
        {
            RunBitTest(5, 0x6B);

        }
        [TestMethod]
        public void TestBit6()
        {
            RunBitTest(6, 0x73);

        }

        [TestMethod]
        public void TestBit7()
        {
            RunBitTest(7, 0x7B);

        }

    }
}
