﻿using System;
using System.Linq.Expressions;
using Emulator.GB.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emulator.GB.Core.Tests.Bits
{
    [TestClass]
    public class TestBitC : TestBit
    {
        protected override Expression<Func<ICpu, byte>> GetRegisterExpression => c => c.C;

        [TestMethod]
        public void TestBit0()
        {
            RunBitTest(0, 0x41);
        }

        [TestMethod]
        public void TestBit1()
        {
            RunBitTest(1, 0x49);

        }

        [TestMethod]
        public void TestBit2()
        {
            RunBitTest(2, 0x51);

        }
        [TestMethod]
        public void TestBit3()
        {
            RunBitTest(3, 0x59);

        }

        [TestMethod]
        public void TestBit4()
        {
            RunBitTest(4, 0x61);

        }

        [TestMethod]
        public void TestBit5()
        {
            RunBitTest(5, 0x69);

        }
        [TestMethod]
        public void TestBit6()
        {
            RunBitTest(6, 0x71);

        }

        [TestMethod]
        public void TestBit7()
        {
            RunBitTest(7, 0x79);

        }

    }
}
