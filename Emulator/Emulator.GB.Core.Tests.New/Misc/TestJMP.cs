using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emulator.GB.Core.Tests.Misc
{
    [TestClass]
    public class TestJMP : BaseTest
    {
        [TestMethod]
        public void TestJMP_NZ()
        {
            _cpu.SetRegister(c => c.PC, 0x53);

            _cpu.Exec(0x20);

            Assert.AreNotEqual(0x55, _cpu.PC);
            Assert.AreEqual(0x53 + 0x53 + 1, _cpu.PC);
        }

        [TestMethod]
        public void TestJMP_Z()
        {
            _cpu.SetRegister(c => c.PC, 0x53);

            _cpu.Exec(0x28);

            Assert.AreEqual(0x54, _cpu.PC);
        }

        [TestMethod]
        public void TestJMP_NC()
        {
            _cpu.SetRegister(c => c.PC, 0x53);

            _cpu.Exec(0x30);

            Assert.AreNotEqual(0x54, _cpu.PC);
        }

        [TestMethod]
        public void TestJMP_C()
        {
            _cpu.SetRegister(c => c.PC, 0x53);

            _cpu.Exec(0x38);

            Assert.AreEqual(0x54, _cpu.PC);
        }

        [TestMethod]
        public void TestJMP_Immediate()
        {
            _cpu.SetRegister(c => c.PC, 0x53);

            _cpu.Exec(0xC3);

            Assert.AreEqual(0x5453, _cpu.PC);
        }
    }
}
