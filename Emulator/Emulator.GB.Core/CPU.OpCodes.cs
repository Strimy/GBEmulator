using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emulator.GB.Core
{
    public partial class CPU
    {
        /// <summary>
        /// Load the value at the program counter address into the specified register
        /// </summary>
        /// <param name="register"></param>
        protected void LoadImmediate(ref byte register)
        {
            register = _mmu.ReadByte(PC++);
            _lastOpTime = 8;
        }

        /// <summary>
        /// Load the value at the program counter address into the specified register
        /// </summary>
        /// <param name="register"></param>
        protected void LoadRegister(ref byte register, byte regValue)
        {
            register = regValue;
            _lastOpTime = 4;
        }

        protected void LoadAddress(ref byte register, int address)
        {
            register = _mmu.ReadByte(address);
            _lastOpTime = 8;
        }

        protected void LoadImmediateAddress(ref byte register)
        {
            var address = _mmu.ReadWord(PC);
            register = _mmu.ReadByte(address);

            PC += 2; // Read word from PC advances the PC from two bytes
            _lastOpTime = 16;
        }

        protected void LoadIntoMemory(int address, byte value)
        {
            _mmu.WriteByte(address, value);

            _lastOpTime = 8;
        }
    }
}
