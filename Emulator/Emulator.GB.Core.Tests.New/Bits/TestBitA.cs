using System;
using System.Linq.Expressions;
using Emulator.GB.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emulator.GB.Core.Tests.Bits
{
    [TestClass]
    public class TestBitA : TestBit
    {
        protected override Expression<Func<ICpu, byte>> GetRegisterExpression => c => c.A;

        [TestMethod]
        public void TestBit0()
        {
            RunBitTest(0, 0x47);
        }

        [TestMethod]
        public void TestBit1()
        {
            RunBitTest(1, 0x4F);

        }

        [TestMethod]
        public void TestBit2()
        {
            RunBitTest(2, 0x57);

        }
        [TestMethod]
        public void TestBit3()
        {
            RunBitTest(3, 0x5F);

        }

        [TestMethod]
        public void TestBit4()
        {
            RunBitTest(4, 0x67);

        }

        [TestMethod]
        public void TestBit5()
        {
            RunBitTest(5, 0x6F);

        }
        [TestMethod]
        public void TestBit6()
        {
            RunBitTest(6, 0x77);

        }

        [TestMethod]
        public void TestBit7()
        {
            RunBitTest(7, 0x7F);

        }

    }
}
