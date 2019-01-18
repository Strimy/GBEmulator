using System;
using Emulator.GB.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emulator.GB.Core.Tests
{
    [TestClass]
    public class Decrement
    {
        private ICpu _cpu;

        public Decrement()
        {
            _cpu = new CPU();
            _cpu.SetMMU(new AddressReturnedMMU());
        }

        [TestMethod]
        public void TestDecA()
        {
            var opcode = 0x3D;
 
            _cpu.SetRegister<byte>(c => c.A, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(4, _cpu.LastOpTime);
            Assert.AreEqual(0x41, _cpu.A);
        }

        [TestMethod]
        public void TestDecB()
        {
            var opcode = 0x05;

            _cpu.SetRegister<byte>(c => c.B, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(4, _cpu.LastOpTime);
            Assert.AreEqual(0x41, _cpu.B);
        }

        [TestMethod]
        public void TestDecC()
        {
            var opcode = 0x0D;
            _cpu.SetRegister<byte>(c => c.C, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(4, _cpu.LastOpTime);
            Assert.AreEqual(0x41, _cpu.C);
        }

        [TestMethod]
        public void TestDecD()
        {
            var opcode = 0x15;

            _cpu.SetRegister<byte>(c => c.D, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(4, _cpu.LastOpTime);
            Assert.AreEqual(0x41, _cpu.D);
        }

        [TestMethod]
        public void TestDecE()
        {
            var opcode = 0x1D;

            _cpu.SetRegister<byte>(c => c.E, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(4, _cpu.LastOpTime);
            Assert.AreEqual(0x41, _cpu.E);
        }

        [TestMethod]
        public void TestDecH()
        {
            var opcode = 0x25;

            _cpu.SetRegister<byte>(c => c.H, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(4, _cpu.LastOpTime);
            Assert.AreEqual(0x41, _cpu.H);
        }

        [TestMethod]
        public void TestDecL()
        {
            var opcode = 0x2D;

            _cpu.SetRegister<byte>(c => c.L, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(4, _cpu.LastOpTime);
            Assert.AreEqual(0x41, _cpu.L);
        }

        [TestMethod]
        public void TestDecBC()
        {
            var opcode = 0x0B;

            _cpu.SetRegister<ushort>(c => c.BC, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x41, _cpu.BC);
        }

        [TestMethod]
        public void TestDecDE()
        {
            var opcode = 0x1B;

            _cpu.SetRegister<ushort>(c => c.DE, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x41, _cpu.DE);
        }

        [TestMethod]
        public void TestDecHL()
        {
            var opcode = 0x2B;

            _cpu.SetRegister<ushort>(c => c.HL, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x41, _cpu.HL);
        }

        [TestMethod]
        public void TestDecSP()
        {
            var opcode = 0x3B;

            _cpu.SetRegister<ushort>(c => c.SP, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x41, _cpu.SP);
        }
    }
}
