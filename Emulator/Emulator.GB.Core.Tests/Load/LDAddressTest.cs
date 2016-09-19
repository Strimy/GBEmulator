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
        public void LD_AA()
        {
            TestRegisterLoad(0x7F, c => c.A, () => _cpu.A);
        }

        [TestMethod]
        public void LD_AB()
        {
            TestRegisterLoad(0x78, c => c.B, () => _cpu.A);
        }

        [TestMethod]
        public void LD_AC()
        {
            TestRegisterLoad(0x79, c => c.C, () => _cpu.A);
        }

        [TestMethod]
        public void LD_AD()
        {
            TestRegisterLoad(0x7A, c => c.D, () => _cpu.A);
        }

        [TestMethod]
        public void LD_AE()
        {
            TestRegisterLoad(0x7B, c => c.E, () => _cpu.A);
        }

        [TestMethod]
        public void LD_AH()
        {
            TestRegisterLoad(0x7C, c => c.H, () => _cpu.A);
        }

        [TestMethod]
        public void LD_AL()
        {
            TestRegisterLoad(0x7D, c => c.L, () => _cpu.A);
        }

        [TestMethod]
        public void LD_AHL()
        {
            _cpu.SetRegister(c => c.HL, (short)0x7008);

            _cpu.Exec(0x7E);

            Assert.AreEqual(8, _cpu.LastOpTime);
            Assert.AreEqual(0x70 ^ 0x08, _cpu.A);            
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

        [TestMethod]
        public void LD_CA()
        {
            TestRegisterLoad(0x4F, c => c.A, () => _cpu.C);
        }

        [TestMethod]
        public void LD_CB()
        {
            TestRegisterLoad(0x48, c => c.B, () => _cpu.C);
        }

        [TestMethod]
        public void LD_CC()
        {
            TestRegisterLoad(0x49, c => c.C, () => _cpu.C);
        }

        [TestMethod]
        public void LD_CD()
        {
            TestRegisterLoad(0x4A, c => c.D, () => _cpu.C);
        }

        [TestMethod]
        public void LD_CE()
        {
            TestRegisterLoad(0x4B, c => c.E, () => _cpu.C);
        }

        [TestMethod]
        public void LD_CH()
        {
            TestRegisterLoad(0x4C, c => c.H, () => _cpu.C);
        }

        [TestMethod]
        public void LD_CL()
        {
            TestRegisterLoad(0x4D, c => c.L, () => _cpu.C);
        }

        [TestMethod]
        public void LD_DA()
        {
            TestRegisterLoad(0x57, c => c.A, () => _cpu.D);
        }

        [TestMethod]
        public void LD_DB()
        {
            TestRegisterLoad(0x50, c => c.B, () => _cpu.D);
        }

        [TestMethod]
        public void LD_DC()
        {
            TestRegisterLoad(0x51, c => c.C, () => _cpu.D);
        }

        [TestMethod]
        public void LD_DD()
        {
            TestRegisterLoad(0x52, c => c.D, () => _cpu.D);
        }

        [TestMethod]
        public void LD_DE()
        {
            TestRegisterLoad(0x53, c => c.E, () => _cpu.D);
        }

        [TestMethod]
        public void LD_DH()
        {
            TestRegisterLoad(0x54, c => c.H, () => _cpu.D);
        }

        [TestMethod]
        public void LD_DL()
        {
            TestRegisterLoad(0x55, c => c.L, () => _cpu.D);
        }

        [TestMethod]
        public void LD_EA()
        {
            TestRegisterLoad(0x5F, c => c.A, () => _cpu.E);
        }

        [TestMethod]
        public void LD_EB()
        {
            TestRegisterLoad(0x58, c => c.B, () => _cpu.E);
        }

        [TestMethod]
        public void LD_EC()
        {
            TestRegisterLoad(0x59, c => c.C, () => _cpu.E);
        }

        [TestMethod]
        public void LD_ED()
        {
            TestRegisterLoad(0x5A, c => c.D, () => _cpu.E);
        }

        [TestMethod]
        public void LD_EE()
        {
            TestRegisterLoad(0x5B, c => c.E, () => _cpu.E);
        }

        [TestMethod]
        public void LD_EH()
        {
            TestRegisterLoad(0x5C, c => c.H, () => _cpu.E);
        }

        [TestMethod]
        public void LD_EL()
        {
            TestRegisterLoad(0x5D, c => c.L, () => _cpu.E);
        }

        [TestMethod]
        public void LD_HA()
        {
            TestRegisterLoad(0x67, c => c.A, () => _cpu.H);
        }

        [TestMethod]
        public void LD_HB()
        {
            TestRegisterLoad(0x60, c => c.B, () => _cpu.H);
        }

        [TestMethod]
        public void LD_HC()
        {
            TestRegisterLoad(0x61, c => c.C, () => _cpu.H);
        }

        [TestMethod]
        public void LD_HD()
        {
            TestRegisterLoad(0x62, c => c.D, () => _cpu.H);
        }

        [TestMethod]
        public void LD_HE()
        {
            TestRegisterLoad(0x63, c => c.E, () => _cpu.H);
        }

        [TestMethod]
        public void LD_HH()
        {
            TestRegisterLoad(0x64, c => c.H, () => _cpu.H);
        }

        [TestMethod]
        public void LD_HL()
        {
            TestRegisterLoad(0x65, c => c.L, () => _cpu.H);
        }

        [TestMethod]
        public void LD_LA()
        {
            TestRegisterLoad(0x6F, c => c.A, () => _cpu.L);
        }

        [TestMethod]
        public void LD_LB()
        {
            TestRegisterLoad(0x68, c => c.B, () => _cpu.L);
        }

        [TestMethod]
        public void LD_LC()
        {
            TestRegisterLoad(0x69, c => c.C, () => _cpu.L);
        }

        [TestMethod]
        public void LD_LD()
        {
            TestRegisterLoad(0x6A, c => c.D, () => _cpu.L);
        }

        [TestMethod]
        public void LD_LE()
        {
            TestRegisterLoad(0x6B, c => c.E, () => _cpu.L);
        }

        [TestMethod]
        public void LD_LH()
        {
            TestRegisterLoad(0x6C, c => c.H, () => _cpu.L);
        }

        [TestMethod]
        public void LD_LL()
        {
            TestRegisterLoad(0x6D, c => c.L, () => _cpu.L);
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
