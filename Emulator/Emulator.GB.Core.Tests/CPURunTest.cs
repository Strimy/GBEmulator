using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Emulator.GB.Core;

namespace Emulator.GB.Core.Tests
{
    [TestClass]
    public class CPURunTest
    {
        [TestMethod]
        public void RunBios()
        {
            CPU cpu = new CPU();
            cpu.SetMMU(new MMU());
            while(cpu.PC < 0xFF)
                cpu.Step();
        }
    }
}
