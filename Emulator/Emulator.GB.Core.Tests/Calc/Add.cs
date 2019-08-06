using System;
using System.Linq.Expressions;
using Emulator.GB.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emulator.GB.Core.Tests
{
    [TestClass]
    public class Add : BaseTest
    {

        private void Add8BitsTest(Expression<Func<ICpu, byte>> expr, int opCode)
        {
            var method = expr.Compile();

            _cpu.SetRegister<byte>(c => c.A, 5);
            _cpu.SetRegister<byte>(expr, 1);
            _cpu.Exec(opCode);
                       
            Assert.AreEqual(false, _cpu.ZeroFlag);
            Assert.AreEqual(false, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.HalfCarryFlag);
            Assert.AreEqual(6, _cpu.A);


            _cpu.SetRegister<byte>(expr, 4);
            _cpu.Exec(opCode);

            Assert.AreEqual(10, _cpu.A);
            Assert.AreEqual(false, _cpu.ZeroFlag);
            Assert.AreEqual(false, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.HalfCarryFlag);

            // Half carry test
            _cpu.SetRegister<byte>(c => c.A, 0x0F);
            _cpu.Exec(opCode);
            Assert.AreEqual(true, _cpu.HalfCarryFlag);
            Assert.AreEqual(false, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.ZeroFlag);
            Assert.AreEqual(false, _cpu.CarryFlag);

            _cpu.SetRegister<byte>(c => c.A, 0xF0);
            _cpu.SetRegister<byte>(expr, 0x10);

            _cpu.Exec(opCode);
            Assert.AreEqual(false, _cpu.HalfCarryFlag);
            Assert.AreEqual(false, _cpu.SubstractFlag);
            Assert.AreEqual(true, _cpu.ZeroFlag);
            Assert.AreEqual(true, _cpu.CarryFlag);

            _cpu.SetRegister<byte>(c => c.A, 0xF0);
            _cpu.SetRegister<byte>(expr, 0x11);

            _cpu.Exec(opCode);
            Assert.AreEqual(false, _cpu.HalfCarryFlag);
            Assert.AreEqual(false, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.ZeroFlag);
            Assert.AreEqual(true, _cpu.CarryFlag);
            Assert.AreEqual(1, _cpu.A);

            Assert.AreEqual(4, _cpu.LastInstructionClockTime);
        }

        [TestMethod]
        public void TestAddA()
        {
            _cpu.SetRegister<byte>(c => c.A, 5);
            _cpu.Exec(0x87);

            Assert.AreEqual(10, _cpu.A);
            Assert.AreEqual(false, _cpu.ZeroFlag);
            Assert.AreEqual(false, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.HalfCarryFlag);

            // Half carry test
            _cpu.SetRegister<byte>(c => c.A, 0x0F);
            _cpu.Exec(0x87);
            Assert.AreEqual(true, _cpu.HalfCarryFlag);
            Assert.AreEqual(false, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.ZeroFlag);
            Assert.AreEqual(false, _cpu.CarryFlag);
            Assert.AreEqual(0x1E, _cpu.A);


            _cpu.SetRegister<byte>(c => c.A, 0xF0);

            _cpu.Exec(0x87);
            Assert.AreEqual(false, _cpu.HalfCarryFlag);
            Assert.AreEqual(false, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.ZeroFlag);
            Assert.AreEqual(true, _cpu.CarryFlag);

            Assert.AreEqual(4, _cpu.LastInstructionClockTime);
        }

        [TestMethod]
        public void TestAddB()
        {
            Add8BitsTest(c => c.B, 0x80);
        }

        [TestMethod]
        public void TestAddC()
        {
            Add8BitsTest(c => c.C, 0x81);
        }

        [TestMethod]
        public void TestAddD()
        {
            Add8BitsTest(c => c.D, 0x82);
        }

        [TestMethod]
        public void TestAddE()
        {
            Add8BitsTest(c => c.E, 0x83);
        }

        [TestMethod]
        public void TestAddH()
        {
            Add8BitsTest(c => c.H, 0x84);
        }

        [TestMethod]
        public void TestAddL()
        {
            Add8BitsTest(c => c.L, 0x85);
        }

        [TestMethod]
        public void TestAddFromHL()
        {
            _cpu.MMU.WriteByte(0xAA10, 0x48);
            _cpu.MMU.WriteByte(0xAA11, 0x1);

            _cpu.SetRegister(o => o.A, (byte)0x01);

            _cpu.SetRegister<ushort>(o => o.HL, 0xAA10);
            _cpu.Exec(0x86);

            Assert.AreEqual(_cpu.A, 0x49);
            Assert.AreEqual(_cpu.ZeroFlag, false);
            Assert.AreEqual(_cpu.SubstractFlag, false);


            _cpu.SetRegister<ushort>(o => o.HL, 0xAA11);
            _cpu.Exec(0x86);

            Assert.AreEqual(_cpu.A, 0x4A);
            Assert.AreEqual(_cpu.ZeroFlag, false);
            Assert.AreEqual(_cpu.SubstractFlag, false);

            _cpu.SetRegister<byte>(c => c.A, 0x0F);
            _cpu.Exec(0x86);
            Assert.AreEqual(true, _cpu.HalfCarryFlag);
            Assert.AreEqual(false, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.ZeroFlag);
            Assert.AreEqual(false, _cpu.CarryFlag);
            Assert.AreEqual(0x10, _cpu.A);

            Assert.AreEqual(8, _cpu.LastInstructionClockTime);

        }

        [TestMethod]
        public void TestAddFromImmediate()
        {
            _cpu.MMU.WriteByte(0x00, 0x48);
            _cpu.MMU.WriteByte(0x01, 0x01);
            _cpu.MMU.WriteByte(0x02, 0x01);
            _cpu.SetRegister(o => o.A, (byte)0x01);

            _cpu.SetRegister<ushort>(o => o.HL, 0xAA10);
            _cpu.Exec(0xC6);

            Assert.AreEqual(_cpu.A, 0x49);
            Assert.AreEqual(_cpu.ZeroFlag, false);
            Assert.AreEqual(_cpu.SubstractFlag, false);


            _cpu.SetRegister<ushort>(o => o.HL, 0xAA11);
            _cpu.Exec(0xC6);

            Assert.AreEqual(_cpu.A, 0x4A);
            Assert.AreEqual(_cpu.ZeroFlag, false);
            Assert.AreEqual(_cpu.SubstractFlag, false);

            _cpu.SetRegister<byte>(c => c.A, 0x0F);
            _cpu.Exec(0xC6);
            Assert.AreEqual(true, _cpu.HalfCarryFlag);
            Assert.AreEqual(false, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.ZeroFlag);
            Assert.AreEqual(false, _cpu.CarryFlag);
            Assert.AreEqual(0x10, _cpu.A);

            Assert.AreEqual(8, _cpu.LastInstructionClockTime);

        }
    }
}
