using Emulator.GB.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Emulator.GB.Core.Tests.Load
{
    [TestClass]
    public class LDAddressTest
    {
        private ICpu _cpu;

        public LDAddressTest()
        {
            _cpu = new CPU();
            _cpu.SetMMU(new AddressReturnedMMU());
        }

        [TestMethod]
        public void LDAA()
        {
            TestRegisterLoad(0x7F, c => c.A, () => _cpu.A);
        }

        [TestMethod]
        public void LDAB()
        {
            TestRegisterLoad(0x78, c => c.B, () => _cpu.A);
        }

        [TestMethod]
        public void LDAC()
        {
            TestRegisterLoad(0x79, c => c.C, () => _cpu.A);
        }

        [TestMethod]
        public void LDAD()
        {
            TestRegisterLoad(0x7A, c => c.D, () => _cpu.A);
        }

        [TestMethod]
        public void LDAE()
        {
            TestRegisterLoad(0x7B, c => c.E, () => _cpu.A);
        }

        [TestMethod]
        public void LDAH()
        {
            TestRegisterLoad(0x7C, c => c.H, () => _cpu.A);
        }

        [TestMethod]
        public void LDAL()
        {
            TestRegisterLoad(0x7D, c => c.L, () => _cpu.A);
        }

        [TestMethod]
        public void LD_BA()
        {
            TestRegisterLoad(0x47, c => c.A, () => _cpu.B);
        }

        [TestMethod]
        public void LD_BB()
        {
            TestRegisterLoad(0x40, c => c.B, () => _cpu.B);
        }

        [TestMethod]
        public void LD_BC()
        {
            TestRegisterLoad(0x41, c => c.C, () => _cpu.B);
        }

        [TestMethod]
        public void LD_BD()
        {
            TestRegisterLoad(0x42, c => c.D, () => _cpu.B);
        }

        [TestMethod]
        public void LD_BE()
        {
            TestRegisterLoad(0x43, c => c.E, () => _cpu.B);
        }

        [TestMethod]
        public void LD_BH()
        {
            TestRegisterLoad(0x44, c => c.H, () => _cpu.B);
        }

        [TestMethod]
        public void LD_BL()
        {
            TestRegisterLoad(0x45, c => c.L, () => _cpu.B);
        }

        private void TestRegisterLoad(int opCode, Expression<Func<ICpu, byte>> source, Func<byte> target)
        {
            _cpu.SetRegister(source, 0x42);

            _cpu.Exec(opCode);

            Assert.AreEqual(4, _cpu.LastOpTime);
            Assert.AreEqual(0x42, target());
        }
    }
}
