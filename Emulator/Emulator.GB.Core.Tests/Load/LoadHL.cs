using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Emulator.GB.Interfaces;

namespace Emulator.GB.Core.Tests.Load
{
    [TestClass]
    public class LoadHL
    {
        private ICpu _cpu;

        public LoadHL()
        {
            _cpu = new CPU();
            _cpu.SetMMU(new AddressReturnedMMU());
        }

        [TestMethod]
        public void TestLoadAFromC()
        {
            _cpu.SetRegister(c => c.C, (byte)0x42);
            _cpu.Exec(0xF2);

            Assert.AreEqual(_cpu.LastOpTime, 8);
            Assert.AreEqual(_cpu.A, 0x42 ^ 0xFF);
        }

        [TestMethod]
        public void TestLoadCFromA()
        {
            _cpu.SetRegister(c => c.C, (byte)0x42);
            _cpu.SetRegister(c => c.A, (byte)0x51);
            _cpu.Exec(0xE2);

            Assert.AreEqual(_cpu.LastOpTime, 8);
            Assert.AreEqual(_cpu.MMU.ReadByte(0xFF00 | 0x42), 0x51);
        }
    }
}
