using Emulator.GB.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Emulator.GB.Core.Tests.Misc
{
    [TestClass]
    public class TestCP : BaseTest
    {
        private void TestCP_REG(Expression<Func<ICpu, byte>> register, int opcode)
        {
            var getter = register.Compile();
            var regValue = getter(_cpu);

            // 0x42 - 0x30
            _cpu.SetRegister<byte>(c => c.A, 0x42);
            _cpu.SetRegister<byte>(register, 0x30);

            _cpu.Exec(opcode);

            
            Assert.AreEqual(opcode == 0xBF, _cpu.ZeroFlag);
            Assert.AreEqual(true, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.CarryFlag);

            _cpu.SetRegister<byte>(register, 0x42);

            _cpu.Exec(opcode);

            Assert.AreEqual(true, _cpu.ZeroFlag);
            Assert.AreEqual(true, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.CarryFlag);

            Assert.AreEqual(4, _cpu.LastOpTime);

        }

        [TestMethod]
        public void TestCPA()
        {
            TestCP_REG(c => c.A, 0xBF);
        }

        [TestMethod]
        public void TestCPB()
        {
            TestCP_REG(c => c.B, 0xB8);
        }

        [TestMethod]
        public void TestCPC()
        {
            TestCP_REG(c => c.C, 0xB9);
        }

        [TestMethod]
        public void TestCPD()
        {
            TestCP_REG(c => c.D, 0xBA);
        }

        [TestMethod]
        public void TestCPE()
        {
            TestCP_REG(c => c.E, 0xBB);
        }

        [TestMethod]
        public void TestCPH()
        {
            TestCP_REG(c => c.H, 0xBC);
        }

        [TestMethod]
        public void TestCPL()
        {
            TestCP_REG(c => c.L, 0xBD);
        }

        [TestMethod]
        public void TestCP_HL_()
        {
            _cpu.SetRegister<byte>(c => c.A, 0x42);
            _cpu.SetRegister<ushort>(c => c.HL, 0x0143);

            _cpu.Exec(0xBE);

            Assert.AreEqual(true, _cpu.ZeroFlag);
            Assert.AreEqual(true, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.CarryFlag);

            _cpu.SetRegister<byte>(c => c.A, 0x42);
            _cpu.SetRegister<ushort>(c => c.HL, 0x0142);

            _cpu.Exec(0xBE);

            Assert.AreEqual(false, _cpu.ZeroFlag);
            Assert.AreEqual(true, _cpu.SubstractFlag);
            Assert.AreEqual(true, _cpu.CarryFlag);
        }

        [TestMethod]
        public void TestCP_Immediate()
        {
            _cpu.SetRegister<byte>(c => c.A, 0x42);
            _cpu.SetRegister(c => c.PC, 0x0143);

            _cpu.Exec(0xFE);

            Assert.AreEqual(true, _cpu.ZeroFlag);
            Assert.AreEqual(true, _cpu.SubstractFlag);
            Assert.AreEqual(false, _cpu.CarryFlag);

            _cpu.Exec(0xFE);

            Assert.AreEqual(false, _cpu.ZeroFlag);
            Assert.AreEqual(true, _cpu.SubstractFlag);
            Assert.AreEqual(true, _cpu.CarryFlag);

            Assert.AreEqual(8, _cpu.LastOpTime);
        }


    }
}
