using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Emulator.GB.Interfaces;

namespace Emulator.GB.Core.Tests.Load
{
    [TestClass]
    public class LoadFFA
    {
        private ICpu _cpu;

        public LoadFFA()
        {
            _cpu = new CPU();
            _cpu.SetMMU(new AddressReturnedMMU());
        }

        [TestMethod]
        public void TestLoadAFromFFImmediate()
        {
            _cpu.MMU.WriteByte(0xFF42, 0x51);

            _cpu.SetRegister(c => c.PC, (byte)0x42);
            _cpu.Exec(0xF0);

            Assert.AreEqual(_cpu.LastOpTime, 12);
            Assert.AreEqual(_cpu.A, 0x51);
        }

        [TestMethod]
        public void TestLoadAToFFImmediate()
        {
            _cpu.SetRegister(c => c.A, (byte)0x51);
            _cpu.SetRegister(c => c.PC, (byte)0x42);
            _cpu.Exec(0xE0);

            Assert.AreEqual(_cpu.LastOpTime, 12);
            Assert.AreEqual(_cpu.MMU.ReadByte(0xFF42), 0x51);
        }
    }
}
