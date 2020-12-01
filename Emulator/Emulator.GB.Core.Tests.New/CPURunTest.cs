using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Emulator.GB.Core;
using Emulator.GB.Core.Cartridge;
using System.IO;
using Emulator.GB.Interfaces;
using System.Diagnostics;

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
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                while (true)
                {
                    cpu.Step();

                    if (cpu.PC > 0xFF)
                        break;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);

            }

            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);

            Assert.AreEqual(0x1, cpu.A);
            Assert.AreEqual(0xB0, cpu.F);
            Assert.AreEqual(0x00, cpu.B);
            Assert.AreEqual(0x13, cpu.C);
            Assert.AreEqual(0x00, cpu.D);
            Assert.AreEqual(0xD8, cpu.E);
            Assert.AreEqual(0x01, cpu.H);
            Assert.AreEqual(0x4D, cpu.L);
            Assert.AreEqual(0xFFFE, cpu.SP);

            Assert.AreEqual(0x00, cpu.MMU.ReadByte(0xFF05));
            Assert.AreEqual(0x00, cpu.MMU.ReadByte(0xFF06));
            Assert.AreEqual(0x00, cpu.MMU.ReadByte(0xFF07));
            // Sound not implemented
            //Assert.AreEqual(0x80, cpu.MMU.ReadByte(0xFF10));
            //Assert.AreEqual(0xBF, cpu.MMU.ReadByte(0xFF11));
            //Assert.AreEqual(0xF3, cpu.MMU.ReadByte(0xFF12));
            //Assert.AreEqual(0xBF, cpu.MMU.ReadByte(0xFF14));
            //Assert.AreEqual(0x3F, cpu.MMU.ReadByte(0xFF16));
            //Assert.AreEqual(0x00, cpu.MMU.ReadByte(0xFF17));
            //Assert.AreEqual(0xBF, cpu.MMU.ReadByte(0xFF19));
            //Assert.AreEqual(0x7F, cpu.MMU.ReadByte(0xFF1A));
            //Assert.AreEqual(0xFF, cpu.MMU.ReadByte(0xFF1B));
            //Assert.AreEqual(0x9F, cpu.MMU.ReadByte(0xFF1C));
            //Assert.AreEqual(0xBF, cpu.MMU.ReadByte(0xFF1E));
            //Assert.AreEqual(0xFF, cpu.MMU.ReadByte(0xFF20));
            //Assert.AreEqual(0x00, cpu.MMU.ReadByte(0xFF21));
            //Assert.AreEqual(0x00, cpu.MMU.ReadByte(0xFF22));
            //Assert.AreEqual(0xBF, cpu.MMU.ReadByte(0xFF23));
            //Assert.AreEqual(0x77, cpu.MMU.ReadByte(0xFF24));
            //Assert.AreEqual(0xF3, cpu.MMU.ReadByte(0xFF25));
            //Assert.AreEqual(0xF1, cpu.MMU.ReadByte(0xFF26));
            Assert.AreEqual(0x91, cpu.MMU.ReadByte(0xFF40));
            Assert.AreEqual(0x00, cpu.MMU.ReadByte(0xFF42));
            Assert.AreEqual(0x00, cpu.MMU.ReadByte(0xFF43));
            Assert.AreEqual(0x00, cpu.MMU.ReadByte(0xFF45));
            Assert.AreEqual(0xFC, cpu.MMU.ReadByte(0xFF47));

            // BGP Not initialized by boot code ?
            //Assert.AreEqual(0xFF, cpu.MMU.ReadByte(0xFF48));
            //Assert.AreEqual(0xFF, cpu.MMU.ReadByte(0xFF49));

            Assert.AreEqual(0x00, cpu.MMU.ReadByte(0xFF4A));
            Assert.AreEqual(0x00, cpu.MMU.ReadByte(0xFF4B));
            Assert.AreEqual(0x00, cpu.MMU.ReadByte(0xFFFF));
        }

        [TestMethod]
        public void TestGPUMode0ToMode2Timing()
        {
            CPU cpu = new CPU(new GPU());
            MMU mmu = new MMU();
            var rom = new ROM();
            cpu.GPU.LCDC = LCDC.LCDEnable;
            cpu.GPU.Mode = GPUMode.Mode0;

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

                if (cpu.InstructionsCount > 100000)
                    Assert.Fail("GPU did not changed to correct state");
            }
        }

        [TestMethod]
        public void TestGPUMode2ToMode3Timing()
        {
            CPU cpu = new CPU(new GPU());
            MMU mmu = new MMU();
            var rom = new ROM();
            cpu.GPU.LCDC = LCDC.LCDEnable;


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
            cpu.GPU.LCDC = LCDC.LCDEnable;


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
            cpu.GPU.LCDC = LCDC.LCDEnable;


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

