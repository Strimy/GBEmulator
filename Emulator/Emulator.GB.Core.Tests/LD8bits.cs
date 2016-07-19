using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Emulator.GB.Interfaces;

namespace Emulator.GB.Core.Tests
{
    [TestClass]
    public class LD8bits
    {
        private ICpu _cpu;

        public LD8bits()
        {
            _cpu = new CPU();
        }

        [TestMethod]
        public void LDBn()
        {
            _cpu.SetRegister(ref _cpu.A, 8);

            _cpu.Exec(0x06);

        }
    }
}
