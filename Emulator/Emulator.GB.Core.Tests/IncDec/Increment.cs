using System;
using Emulator.GB.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emulator.GB.Core.Tests
{
    [TestClass]
    public class Increment
    {
        private ICpu _cpu;

        public Increment()
        {
            _cpu = new CPU();
            _cpu.SetMMU(new AddressReturnedMMU());
        }

        [TestMethod]
        public void TestIncA()
        {
            var opcode = 0x3C;
 
            _cpu.SetRegister<byte>(c => c.A, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(4, _cpu.LastOpTime);
            Assert.AreEqual(0x43, _cpu.A);
        }

        [TestMethod]
        public void TestIncB()
        {
            var opcode = 0x04;

            _cpu.SetRegister<byte>(c => c.B, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(4, _cpu.LastOpTime);
            Assert.AreEqual(0x43, _cpu.B);
        }

        [TestMethod]
        public void TestIncC()
        {
            var opcode = 0x0C;
            _cpu.SetRegister<byte>(c => c.C, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(4, _cpu.LastOpTime);
            Assert.AreEqual(0x43, _cpu.C);
        }

        [TestMethod]
        public void TestIncD()
        {
            var opcode = 0x14;

            _cpu.SetRegister<byte>(c => c.D, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(4, _cpu.LastOpTime);
            Assert.AreEqual(0x43, _cpu.D);
        }

        [TestMethod]
        public void TestIncE()
        {
            var opcode = 0x1C;

            _cpu.SetRegister<byte>(c => c.E, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(4, _cpu.LastOpTime);
            Assert.AreEqual(0x43, _cpu.E);
        }

        [TestMethod]
        public void TestIncH()
        {
            var opcode = 0x24;

            _cpu.SetRegister<byte>(c => c.H, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(4, _cpu.LastOpTime);
            Assert.AreEqual(0x43, _cpu.H);
        }

        [TestMethod]
        public void TestIncL()
        {
            var opcode = 0x2c;

            _cpu.SetRegister<byte>(c => c.L, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(4, _cpu.LastOpTime);
            Assert.AreEqual(0x43, _cpu.L);
        }

        [TestMethod]
        public void TestIncBC()
        {
            var opcode = 0x03;

            // Low case
            _cpu.SetRegister<ushort>(c => c.BC, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x43, _cpu.BC);

            // High case
            _cpu.SetRegister<ushort>(c => c.BC, 0x42FF);

            _cpu.Exec(opcode);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x4300, _cpu.BC);
        }

        [TestMethod]
        public void TestIncDE()
        {
            var opcode = 0x13;

            _cpu.SetRegister<ushort>(c => c.DE, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x43, _cpu.DE);

            // High case
            _cpu.SetRegister<ushort>(c => c.DE, 0x42FF);

            _cpu.Exec(opcode);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x4300, _cpu.DE);
        }

        [TestMethod]
        public void TestIncHL()
        {
            var opcode = 0x23;

            _cpu.SetRegister<ushort>(c => c.HL, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x43, _cpu.HL);

            // High case
            _cpu.SetRegister<ushort>(c => c.HL, 0x42FF);

            _cpu.Exec(opcode);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x4300, _cpu.HL);
        }

        [TestMethod]
        public void TestIncSP()
        {
            var opcode = 0x33;

            _cpu.SetRegister<ushort>(c => c.SP, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x43, _cpu.SP);

            // High case
            _cpu.SetRegister<ushort>(c => c.SP, 0x42FF);

            _cpu.Exec(opcode);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x4300, _cpu.SP);
        }
    }
}
