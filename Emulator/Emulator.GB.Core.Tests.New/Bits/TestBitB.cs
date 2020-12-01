using System;
using System.Linq.Expressions;
using Emulator.GB.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emulator.GB.Core.Tests.Bits
{
    [TestClass]
    public class TestBitB : TestBit
    {
        protected override Expression<Func<ICpu, byte>> GetRegisterExpression => c => c.B;

        [TestMethod]
        public void TestBit0()
        {
            RunBitTest(0, 0x40);
        }

        [TestMethod]
        public void TestBit1()
        {
            RunBitTest(1, 0x48);

        }

        [TestMethod]
        public void TestBit2()
        {
            RunBitTest(2, 0x50);

        }
        [TestMethod]
        public void TestBit3()
        {
            RunBitTest(3, 0x58);

        }

        [TestMethod]
        public void TestBit4()
        {
            RunBitTest(4, 0x60);

        }

        [TestMethod]
        public void TestBit5()
        {
            RunBitTest(5, 0x68);

        }
        [TestMethod]
        public void TestBit6()
        {
            RunBitTest(6, 0x70);

        }

        [TestMethod]
        public void TestBit7()
        {
            RunBitTest(7, 0x78);

        }

    }
}
