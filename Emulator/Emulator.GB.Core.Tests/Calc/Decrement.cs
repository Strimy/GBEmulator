using System;
using System.Linq.Expressions;
using Emulator.GB.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emulator.GB.Core.Tests
{
    [TestClass]
    public class Decrement : BaseTest
    {

        private void Dec8BitsTest(Expression<Func<ICpu, byte>> expr, int opCode)
        {
            var method = expr.Compile();

            _cpu.SetRegister<byte>(expr, 0x42);
            _cpu.Exec(opCode);
                       
            Assert.AreEqual(0x41, method(_cpu));
            Assert.AreEqual(false, _cpu.ZeroFlag);
            Assert.AreEqual(true, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.HalfCarryFlag);

            _cpu.SetRegister<byte>(expr, 0x01);
            _cpu.Exec(opCode);

            Assert.AreEqual(0, method(_cpu));
            Assert.AreEqual(true, _cpu.ZeroFlag);
            Assert.AreEqual(true, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.HalfCarryFlag);

            _cpu.SetRegister<byte>(expr, 0x10);
            _cpu.Exec(opCode);

            Assert.AreEqual(0xF, method(_cpu));
            Assert.AreEqual(false, _cpu.ZeroFlag);
            Assert.AreEqual(true, _cpu.SubstractFlag);
            Assert.AreEqual(true, _cpu.HalfCarryFlag);

            Assert.AreEqual(4, _cpu.LastInstructionClockTime);

        }

        [TestMethod]
        public void TestDecA()
        {
            Dec8BitsTest(c => c.A, 0x3D);
        }

        [TestMethod]
        public void TestDecB()
        {
            Dec8BitsTest(c => c.B, 0x05);

        }

        [TestMethod]
        public void TestDecC()
        {
            Dec8BitsTest(c => c.C, 0x0D);

        }

        [TestMethod]
        public void TestDecD()
        {
            Dec8BitsTest(c => c.D, 0x15);

        }

        [TestMethod]
        public void TestDecE()
        {
            Dec8BitsTest(c => c.E, 0x1D);
        }

        [TestMethod]
        public void TestDecH()
        {
            Dec8BitsTest(c => c.H, 0x25);
        }

        [TestMethod]
        public void TestDecL()
        {
            Dec8BitsTest(c => c.L, 0x2D);
        }

        [TestMethod]
        public void TestDecBC()
        {
            var opcode = 0x0B;

            _cpu.SetRegister<ushort>(c => c.BC, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(8, _cpu.LastInstructionClockTime);
            Assert.AreEqual(0x41, _cpu.BC);
        }

        [TestMethod]
        public void TestDecDE()
        {
            var opcode = 0x1B;

            _cpu.SetRegister<ushort>(c => c.DE, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(8, _cpu.LastInstructionClockTime);
            Assert.AreEqual(0x41, _cpu.DE);
        }

        [TestMethod]
        public void TestDecHL()
        {
            var opcode = 0x2B;

            _cpu.SetRegister<ushort>(c => c.HL, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(8, _cpu.LastInstructionClockTime);
            Assert.AreEqual(0x41, _cpu.HL);

            _cpu.SetRegister<ushort>(c => c.HL, 0x0100);
            _cpu.Exec(opcode);
            Assert.AreEqual(0xFF, _cpu.HL);
        }

        [TestMethod]
        public void TestDecSP()
        {
            var opcode = 0x3B;

            _cpu.SetRegister<ushort>(c => c.SP, 0xFF42);
            _cpu.Exec(opcode);
            Assert.AreEqual(0xFF41, _cpu.SP);

            _cpu.SetRegister<ushort>(c => c.SP, 0x0100);
            _cpu.Exec(opcode);
            Assert.AreEqual(0xFF, _cpu.SP);

            Assert.AreEqual(8, _cpu.LastInstructionClockTime);
        }
    }
}
