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
        /// <param name="register">Target register</param>
        protected void LoadImmediate(ref byte register)
        {
            register = _mmu.ReadByte(PC++);

            // This is the combination of MMU read and the effective load into the register
            _lastOpTime = 4 + 4;
        }

        /// <summary>
        /// Load the value from a register to another register
        /// </summary>
        /// <param name="register">Target register</param>
        /// <param name="regValue">Value to load into register</param>
        protected void LoadRegister(ref byte register, byte regValue)
        {
            register = regValue;
            _lastOpTime = 4;
        }

        /// <summary>
        /// Load byte value stored at address into register
        /// </summary>
        /// <param name="register">Target register</param>
        /// <param name="address">Address to read the byte from</param>
        protected void LoadAddress(ref byte register, int address)
        {
            register = _mmu.ReadByte(address);
            _lastOpTime = 8; // MMU + Load
        }


        /// <summary>
        /// Load byte value at the address written at the PC
        /// </summary>
        /// <param name="register">Target register</param>
        protected void LoadImmediateAddress(ref byte register)
        {
            var address = _mmu.ReadWord(PC);
            register = _mmu.ReadByte(address);

            PC += 2; // Read word from PC advances the PC from two bytes
            _lastOpTime = 16;
        }

        /// <summary>
        /// Write the value into memory
        /// </summary>
        /// <param name="address">Address to write the value to</param>
        /// <param name="value">Value</param>
        protected void LoadIntoMemory(int address, byte value)
        {
            _mmu.WriteByte(address, value);

            _lastOpTime = 8;
        }


        protected void LoadAFromC()
        {
            _a = _mmu.ReadByte(0xFF00 | _c);
            _lastOpTime = 8;
        }

        protected void LoadCFromA()
        {
            _mmu.WriteByte(0xFF00 + _c, _a);
            _lastOpTime = 8;
        }
    }
}
