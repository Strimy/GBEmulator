using System;
using System.Linq.Expressions;
using Emulator.GB.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emulator.GB.Core.Tests
{
    [TestClass]
    public class Rotate : BaseTest
    {

        private void RLTest(Expression<Func<ICpu, byte>> expr, int opCodeExt)
        {
            var method = expr.Compile();

            _cpu.SetRegister<byte>(expr, 0x42);
            _cpu.ExecExtOpCode(opCodeExt);

            Assert.AreEqual(0x42 * 2, method(_cpu));
            Assert.AreEqual(false, _cpu.ZeroFlag);
            Assert.AreEqual(false, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.HalfCarryFlag);

            _cpu.SetRegister<byte>(expr, 0x80);
            _cpu.ExecExtOpCode(opCodeExt);

            Assert.AreEqual(0, method(_cpu));
            Assert.AreEqual(true, _cpu.ZeroFlag);
            Assert.AreEqual(true, _cpu.CarryFlag);
            Assert.AreEqual(false, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.HalfCarryFlag);

            _cpu.SetRegister<byte>(expr, 0);
            _cpu.ExecExtOpCode(opCodeExt);

            Assert.AreEqual(1, method(_cpu)); // Carry flag used
            Assert.AreEqual(false, _cpu.ZeroFlag);
            Assert.AreEqual(false, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.CarryFlag);
            Assert.AreEqual(false, _cpu.HalfCarryFlag);

            _cpu.SetRegister<byte>(expr, 0);
            _cpu.ExecExtOpCode(opCodeExt);

            Assert.AreEqual(0, method(_cpu)); // Carry flag used
            Assert.AreEqual(true, _cpu.ZeroFlag);
            Assert.AreEqual(false, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.CarryFlag);
            Assert.AreEqual(false, _cpu.HalfCarryFlag);

            Assert.AreEqual(8, _cpu.LastInstructionClockTime);

        }
        
        [TestMethod]
        public void TestRLA()
        {
            RLTest(c => c.A, 0x17);
        }

        [TestMethod]
        public void TestRLB()
        {
            RLTest(c => c.B, 0x10);

        }

        [TestMethod]
        public void TestRLC()
        {
            RLTest(c => c.C, 0x11);
        }

        [TestMethod]
        public void TestRLD()
        {
            RLTest(c => c.D, 0x12);

        }

        [TestMethod]
        public void TestRLE()
        {
            RLTest(c => c.E, 0x13);
        }

        [TestMethod]
        public void TestRLH()
        {
            RLTest(c => c.H, 0x14);
        }

        [TestMethod]
        public void TestRLL()
        {
            RLTest(c => c.L, 0x15);
        }

        [TestMethod]
        public void TestRL_HL_()
        {
            _cpu.MMU.WriteByte(0x4242, 0x32);
            _cpu.SetRegister<ushort>(c => c.HL, 0x4242);
            _cpu.ExecExtOpCode(0x16);

            Assert.AreEqual(0x32 * 2, _cpu.MMU.ReadByte(0x4242));

            Assert.AreEqual(16, _cpu.LastInstructionClockTime, "Bad op time");

        }
    }
}
