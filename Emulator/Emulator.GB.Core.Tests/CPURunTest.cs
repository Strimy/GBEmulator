using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Emulator.GB.Core;
using Emulator.GB.Core.Cartridge;
using System.IO;
using Emulator.GB.Interfaces;

namespace Emulator.GB.Core.Tests.Run
{
    [TestClass]
    public class CPURunTest
    {
        [TestMethod]
        public void RunBios()
        {
            CPU cpu = new CPU(new GPU());
            MMU mmu = new MMU();
            var rom = new ROM();

            rom.LoadROM(File.ReadAllBytes("opus5.gb"));
            mmu.SetCartridge(rom);
            cpu.SetMMU(mmu);

            while (cpu.PC < 0xFF)
            {
                cpu.Step();

                if(cpu.InstructionsCount > 10000000)
                {
                    Assert.Fail("Boot time exceeded");
                }
            }

            Assert.Fail(cpu.ToString());
        }

        [TestMethod]
        public void TestGPUMode0ToMode2Timing()
        {
            CPU cpu = new CPU(new GPU());
            MMU mmu = new MMU();
            var rom = new ROM();


            decimal timeSpentMicroSec = 0;

            GPUMode gpuMode = cpu.GPU.Mode;


            while (true)
            {
                cpu.Exec(0x00);

                timeSpentMicroSec += (decimal)1.0 / cpu.ClockFrequencyMhz * cpu.LastInstructionClockTime;

                var newMode = cpu.GPU.Mode;
                if (gpuMode != newMode)
                {
                    if (newMode == GPUMode.Mode2)
                    {
                        // We got the new mode, check that timings are ok
                        // Mode0 should stay up for 204 clock, meaning 48.6µsec
                        Assert.IsTrue(timeSpentMicroSec > 48.5M);
                        Assert.IsTrue(timeSpentMicroSec < 48.7M);

                        return;
                    }
                    else
                        Assert.Fail("GPU Mode should transition from Mode0 to Mode2");
                }

                if (cpu.InstructionsCount > 10000)
                    Assert.Fail("GPU did not changed to correct state");
            }
        }

        [TestMethod]
        public void TestGPUMode2ToMode3Timing()
        {
            CPU cpu = new CPU(new GPU());
            MMU mmu = new MMU();
            var rom = new ROM();


            decimal timeSpentMicroSec = 0;


            while (cpu.GPU.Mode != GPUMode.Mode2)
            {
                cpu.Exec(0x00);

                if (cpu.InstructionsCount > 10000)
                    Assert.Fail("GPU did not changed to correct state");
            }

            GPUMode gpuMode = cpu.GPU.Mode;

            while (true)
            {
                cpu.Exec(0x00);

                timeSpentMicroSec += (decimal)1.0 / cpu.ClockFrequencyMhz * cpu.LastInstructionClockTime;

                var newMode = cpu.GPU.Mode;
                if (gpuMode != newMode)
                {
                    if (newMode == GPUMode.Mode3)
                    {
                        // We got the new mode, check that timings are ok
                        // Mode0 should stay up for 204 clock, meaning 19µsec
                        Assert.IsTrue(timeSpentMicroSec > 19M);
                        Assert.IsTrue(timeSpentMicroSec < 19.1M);
                        return;
                    }
                    else
                        Assert.Fail("GPU Mode should transition from Mode0 to Mode2");
                }

                if (cpu.InstructionsCount > 10000)
                    Assert.Fail("GPU did not changed to correct state");
            }
        }

        [TestMethod]
        public void TestGPUMode3ToMode0Timing()
        {
            CPU cpu = new CPU(new GPU());
            MMU mmu = new MMU();
            var rom = new ROM();


            decimal timeSpentMicroSec = 0;


            while (cpu.GPU.Mode != GPUMode.Mode3)
            {
                cpu.Exec(0x00);

                if (cpu.InstructionsCount > 10000)
                    Assert.Fail("GPU did not changed to correct state");
            }

            GPUMode gpuMode = cpu.GPU.Mode;

            while (true)
            {
                cpu.Exec(0x00);

                timeSpentMicroSec += (decimal)1.0 / cpu.ClockFrequencyMhz * cpu.LastInstructionClockTime;

                var newMode = cpu.GPU.Mode;
                if (gpuMode != newMode)
                {
                    if (newMode == GPUMode.Mode0)
                    {
                        // We got the new mode, check that timings are ok
                        // Mode3 should stay up for 204 clock, meaning 19µsec
                        Assert.IsTrue(timeSpentMicroSec > 41);
                        Assert.IsTrue(timeSpentMicroSec < 42M);
                        return;
                    }
                    else
                        Assert.Fail("GPU Mode should transition from Mode0 to Mode2");
                }

                if (cpu.InstructionsCount > 10000)
                    Assert.Fail("GPU did not changed to correct state");
            }
        }

        [TestMethod]
        public void TestGPUVBlankTiming()
        {
            CPU cpu = new CPU(new GPU());
            MMU mmu = new MMU();
            var rom = new ROM();


            decimal timeSpentMicroSec = 0;

            // Wait for the first VBlank
            while (cpu.GPU.Mode != GPUMode.Mode1)
            {
                cpu.Exec(0x00);

                if (cpu.InstructionsCount > 100000)
                    Assert.Fail("GPU did not changed to correct state");
            }

            GPUMode gpuMode = cpu.GPU.Mode;

            var mode1ClockCount = 0;


            while (true)
            {
                cpu.Exec(0x00);

                timeSpentMicroSec += (decimal)1.0 / cpu.ClockFrequencyMhz * cpu.LastInstructionClockTime;
                mode1ClockCount += cpu.LastInstructionClockTime;

                var newMode = cpu.GPU.Mode;
                if (gpuMode != newMode)
                {
                    
                    if (newMode == GPUMode.Mode1)
                    {
                        Assert.AreEqual(70224, mode1ClockCount);

                        Assert.IsTrue(timeSpentMicroSec > 16700M);
                        Assert.IsTrue(timeSpentMicroSec < 16800M);

                        // VBlank frequency is around 59.73Hz
                        var vsync = 1 / (double)timeSpentMicroSec * 1000.0 * 1000.0;
                        Assert.IsTrue(vsync > 59.7);
                        Assert.IsTrue(vsync < 59.8);

                        return;
                    }
                    else if(newMode == GPUMode.Mode2 && gpuMode == GPUMode.Mode1)
                    {
                        Assert.AreEqual(4560, mode1ClockCount);
                        // Mode 1 should stay up for 1.08msec
                        Assert.IsTrue(timeSpentMicroSec > 1080M);
                        Assert.IsTrue(timeSpentMicroSec < 1090M);
                    }
                    gpuMode = newMode;
                }

                if (cpu.InstructionsCount > 1000000)
                    Assert.Fail("GPU did not changed to correct state");
            }
        }
    }
}

