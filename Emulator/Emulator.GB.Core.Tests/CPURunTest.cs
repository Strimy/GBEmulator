using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Emulator.GB.Core;
using Emulator.GB.Core.Cartridge;
using System.IO;

namespace Emulator.GB.Core.Tests
{
    [TestClass]
    public class CPURunTest
    {
        [TestMethod]
        public void RunBios()
        {
            CPU cpu = new CPU();
            MMU mmu = new MMU();
            var rom = new ROM();

            rom.LoadROM(File.ReadAllBytes("opus5.gb"));
            mmu.SetCartridge(rom);
            cpu.SetMMU(mmu);

            while(cpu.PC < 0xFF)
                cpu.Step();

            Assert.Fail(cpu.ToString());
        }
    }
}
