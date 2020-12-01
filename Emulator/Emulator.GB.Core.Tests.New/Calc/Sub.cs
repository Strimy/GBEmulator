using System;
using System.Linq.Expressions;
using Emulator.GB.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emulator.GB.Core.Tests
{
    [TestClass]
    public class Sub : BaseTest
    {

        private void Sub8BitsTest(Expression<Func<ICpu, byte>> expr, int opCode)
        {
            var method = expr.Compile();

            _cpu.SetRegister<byte>(c => c.A, 5);
            if(opCode != 0x97)
            {
                _cpu.SetRegister<byte>(expr, 1);
                _cpu.Exec(opCode);
                       
                Assert.AreEqual(false, _cpu.ZeroFlag);
                Assert.AreEqual(true, _cpu.SubstractFlag);
                Assert.AreEqual(false, _cpu.HalfCarryFlag);
                Assert.AreEqual(4, _cpu.A);
            }


            _cpu.SetRegister<byte>(expr, 4);
            _cpu.Exec(opCode);

            Assert.AreEqual(0, _cpu.A);
            Assert.AreEqual(true, _cpu.ZeroFlag);
            Assert.AreEqual(true, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.HalfCarryFlag);

            Assert.AreEqual(4, _cpu.LastInstructionClockTime);
        }

        [TestMethod]
        public void TestSubA()
        {
            Sub8BitsTest(c => c.A, 0x97);
        }

        [TestMethod]
        public void TestSubB()
        {
            Sub8BitsTest(c => c.B, 0x90);
        }

        [TestMethod]
        public void TestSubC()
        {
            Sub8BitsTest(c => c.C, 0x91);
        }

        [TestMethod]
        public void TestSubD()
        {
            Sub8BitsTest(c => c.D, 0x92);
        }

        [TestMethod]
        public void TestSubE()
        {
            Sub8BitsTest(c => c.E, 0x93);
        }

        [TestMethod]
        public void TestSubH()
        {
            Sub8BitsTest(c => c.H, 0x94);
        }

        [TestMethod]
        public void TestSubL()
        {
            Sub8BitsTest(c => c.L, 0x95);
        }

        [TestMethod]
        public void TestSubFromHL()
        {
            _cpu.MMU.WriteByte(0xAA10, 0x48);
            _cpu.MMU.WriteByte(0xAA11, 0x1);

            _cpu.SetRegister(o => o.A, (byte)0x49);

            _cpu.SetRegister<ushort>(o => o.HL, 0xAA10);
            _cpu.Exec(0x96);

            Assert.AreEqual(_cpu.A, 1);
            Assert.AreEqual(_cpu.ZeroFlag, false);
            Assert.AreEqual(_cpu.SubstractFlag, true);

            _cpu.SetRegister<ushort>(o => o.HL, 0xAA11);
            _cpu.Exec(0x96);

            Assert.AreEqual(_cpu.A, 0);
            Assert.AreEqual(_cpu.ZeroFlag, true);
            Assert.AreEqual(_cpu.SubstractFlag, true);

        }

        [TestMethod]
        public void TestSubFromImmediate()
        {
            _cpu.MMU.WriteByte(0x00, 0x48);
            _cpu.MMU.WriteByte(0x01, 0x01);
            _cpu.SetRegister(o => o.A, (byte)0x49);

            _cpu.Exec(0xD6);

            Assert.AreEqual(_cpu.A, 1);
            Assert.AreEqual(_cpu.ZeroFlag, false);
            Assert.AreEqual(_cpu.SubstractFlag, true);

            _cpu.Exec(0xD6);

            Assert.AreEqual(_cpu.A, 0);
            Assert.AreEqual(_cpu.ZeroFlag, true);
            Assert.AreEqual(_cpu.SubstractFlag, true);

        }
    }
}
