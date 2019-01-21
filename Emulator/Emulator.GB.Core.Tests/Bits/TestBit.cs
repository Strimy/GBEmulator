using System;
using System.Linq.Expressions;
using Emulator.GB.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emulator.GB.Core.Tests.Bits
{
    [TestClass]
    public abstract class TestBit
    {
        protected ICpu _cpu;

        protected abstract Expression<Func<ICpu, byte>> GetRegisterExpression { get; }

        public TestBit()
        {
            _cpu = new CPU();
            _cpu.SetMMU(new AddressReturnedMMU());
        }

        protected void RunBitTest(int bitPos, int opCodeExt)
        {

            for (byte i = 0; i < 0xFF; i++)
            {
                bool result = ((i >> bitPos) & 0x01) == 1;
                _cpu.SetRegister<byte>(GetRegisterExpression, i);
                _cpu.ExecExtOpCode(opCodeExt);

                Assert.AreNotEqual(result, _cpu.ZeroFlag);
                Assert.AreEqual(false, _cpu.SubstractFlag);
                Assert.AreEqual(true, _cpu.HalfCarryFlag);
                Assert.AreEqual(8, _cpu.LastOpTime);
            }


        }
    }
}
