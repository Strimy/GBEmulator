using System;
using System.Linq.Expressions;
using Emulator.GB.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emulator.GB.Core.Tests.Bits
{
    [TestClass]
    public class TestXor
    {
        protected ICpu _cpu;
        public TestXor()
        {
            _cpu = new CPU();
            _cpu.SetMMU(new AddressReturnedMMU());
        }

        private void XorTest(Expression<Func<ICpu, byte>> expr, int opCode)
        {
            var method = expr.Compile();

            _cpu.SetRegister<byte>(c => c.A, 0x42);
            _cpu.SetRegister<byte>(expr, 0x42);
            Func<byte> resultFn = () => { return (byte)(_cpu.A ^ method(_cpu)); };
            var result = resultFn();
            _cpu.Exec(opCode);


            Assert.AreEqual(result, _cpu.A);
            Assert.AreEqual(true, _cpu.ZeroFlag);
            Assert.AreEqual(false, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.HalfCarryFlag);



            _cpu.SetRegister<byte>(expr, 0x01);
            result = resultFn();
            _cpu.Exec(opCode);

            Assert.AreEqual(result, _cpu.A);
            Assert.AreEqual(result == 0, _cpu.ZeroFlag);
            Assert.AreEqual(false, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.HalfCarryFlag);


            Assert.AreEqual(4, _cpu.LastOpTime);

        }

        [TestMethod]
        public void TestXOR_A()
        {
            XorTest(c => c.A, 0xAF);
        }

        [TestMethod]
        public void TestXOR_B()
        {
            XorTest(c => c.B, 0xA8);
        }

        [TestMethod]
        public void TestXOR_C()
        {
            XorTest(c => c.C, 0xA9);
        }

        [TestMethod]
        public void TestXOR_D()
        {
            XorTest(c => c.D, 0xAA);
        }

        [TestMethod]
        public void TestXOR_E()
        {
            XorTest(c => c.E, 0xAB);
        }

        [TestMethod]
        public void TestXOR_H()
        {
            XorTest(c => c.H, 0xAC);
        }
        [TestMethod]
        public void TestXOR_L()
        {
            XorTest(c => c.L, 0xAD);
        }

        [TestMethod]
        public void TestHL()
        {
        }

    }
}
