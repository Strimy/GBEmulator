using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Emulator.GB.Interfaces;
using System.Linq.Expressions;

namespace Emulator.GB.Core.Tests
{
    [TestClass]
    public class LD8bits
    {
        private ICpu _cpu;

        public LD8bits()
        {
            _cpu = new CPU();
            _cpu.SetMMU(new AddressReturnedMMU());
        }

        [TestMethod]
        public void LD_An()
        {
            _cpu.SetRegister(c => c.PC, 0x42);

            _cpu.Exec(0x3E);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x42, _cpu.A);
        }

        [TestMethod]
        public void LD_Bn()
        {
            _cpu.SetRegister(c => c.PC, 0x42);

            _cpu.Exec(0x06);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x42, _cpu.B);
        }

        [TestMethod]
        public void LD_Cn()
        {
            _cpu.SetRegister(c => c.PC, 0x42);

            _cpu.Exec(0x0E);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x42, _cpu.C);
        }

        [TestMethod]
        public void LD_Dn()
        {
            _cpu.SetRegister(c => c.PC, 0x42);

            _cpu.Exec(0x16);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x42, _cpu.D);
        }

        [TestMethod]
        public void LD_En()
        {
            _cpu.SetRegister(c => c.PC, 0x42);

            _cpu.Exec(0x1E);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x42, _cpu.E);
        }


        [TestMethod]
        public void LD_Hn()
        {
            _cpu.SetRegister(c => c.PC, 0x42);

            _cpu.Exec(0x26);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x42, _cpu.H);
        }

        [TestMethod]
        public void LD_Ln()
        {
            _cpu.SetRegister(c => c.PC, 0x42);

            _cpu.Exec(0x2E);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x42, _cpu.L);
        }
    }
}
