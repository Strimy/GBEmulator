using Emulator.GB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator.GB.Core.Tests
{
    public abstract class BaseTest
    {
        protected ICpu _cpu;

        public BaseTest()
        {
            _cpu = new CPU(new GPU());
            _cpu.SetMMU(new AddressReturnedMMU());
        }
    }
}
