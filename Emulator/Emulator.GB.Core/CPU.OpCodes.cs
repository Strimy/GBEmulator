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
        /// Load word value stored at address into register
        /// </summary>
        /// <param name="register">Target register</param>
        /// <param name="address">Address to read the byte from</param>
        protected void LoadImmediate16Bits(ref ushort register)
        {
            register = _mmu.ReadWord(PC);
            _lastOpTime = 12;
            PC += 2;
        }

        /// <summary>
        /// Load word value stored at address into register
        /// </summary>
        /// <param name="register">Target register</param>
        /// <param name="address">Address to read the byte from</param>
        protected void LoadImmediate16Bits(ref byte hregister, ref byte lregister)
        {
            lregister = _mmu.ReadByte(PC++);
            hregister = _mmu.ReadByte(PC++);
            _lastOpTime = 12;
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

        protected void Xor(ref byte register)
        {
            _a = (byte)(register ^ _a);

            HalfCarryFlag = false;
            ZeroFlag = _a == 0;
            SubstractFlag = false;
            CarryFlag = false;

            _lastOpTime = 4;
            // TODO : Set flags Z, N, H, C
        }

        protected void Xor(int address)
        {
            var value = _mmu.ReadByte(address);
            _a = (byte)(value ^ _a);

            HalfCarryFlag = false;
            ZeroFlag = _a == 0;
            SubstractFlag = false;
            CarryFlag = false;

            _lastOpTime = 8;
            // TODO : Set flags Z, N, H, C
        }

        protected void TestBit(int bitPos, byte value)
        {
            ZeroFlag = ((value >> bitPos) & 0x1) == 0;
            SubstractFlag = false;
            HalfCarryFlag = true;

            _lastOpTime = 8;
        }

        protected void JmpNZ()
        {
            if(!ZeroFlag)
            {
                JumpImmediateRelative();
            }
            else
            {
                PC++;
            }
        }

        protected void JmpZ()
        {
            if (ZeroFlag)
            {
                JumpImmediateRelative();
            }
            else
            {
                PC++;
            }
        }

        protected void JmpNC()
        {
            if (!CarryFlag)
            {
                JumpImmediateRelative();
            }
            else
            {
                PC++;
            }
        }

        protected void JmpC()
        {
            if (CarryFlag)
            {
                JumpImmediateRelative();
            }
            else
            {
                PC++;
            }
        }


        protected void JumpImmediateRelative()
        {
            var realValue = _mmu.ReadByte(PC++);
            sbyte offset = (sbyte)realValue;
            PC += offset;
            _lastOpTime = 8;
        }

        protected void JumpImmediate()
        {
            var realValue = _mmu.ReadWord(PC++);
            PC = realValue;

            _lastOpTime = 12;
        }

        protected void Increment(ref byte register)
        {
            register++;
            HalfCarryFlag = register > 0x0F;
            SubstractFlag = false;
            ZeroFlag = register == 0;
            _lastOpTime = 4;
        }

        protected void Increment(ref byte h, ref byte l)
        {
            unchecked
            {
                l++;
                if (l == 0)
                    h++;
            }
            _lastOpTime = 8;
        }

        protected void Increment(ref ushort _register)
        {
            _register++;
            _lastOpTime = 8;
        }

        protected void Decrement(ref byte register)
        {
            register--;
            HalfCarryFlag = (register & 0x0F) == 0x0F; // Not sure how the Half carry flag is working
            _lastOpTime = 4;
            SubstractFlag = true;
            ZeroFlag = register == 0;
        }

        protected void Decrement(ref byte h, ref byte l)
        {
            l--;
            if (l == 0xFF)
                h--;

            _lastOpTime = 8;
        }

        protected void Decrement(ref ushort _register)
        {
            _register--;
            _lastOpTime = 8;
        }

        protected void Add(byte register)
        {
            int localA = register + _a;

            HalfCarryFlag = ((_a & 0x0F) + (register & 0x0F)) > 0x0F;

            _a = (byte)(localA & 0xFF);

            CarryFlag = localA > 0xFF;
            SubstractFlag = false;
            ZeroFlag = _a == 0;
            _lastOpTime = 4;
        }

        protected void AddFromHL()
        {
            var value = MMU.ReadByte(HL);
            int localA = value + _a;

            HalfCarryFlag = ((_a & 0x0F) + (value & 0x0F)) > 0x0F;

            _a = (byte)(localA & 0xFF);

            CarryFlag = localA > 0xFF;
            SubstractFlag = false;
            ZeroFlag = _a == 0;
            _lastOpTime = 8;
        }

        protected void AddFromImmediate()
        {
            var value = MMU.ReadByte(PC++);
            int localA = value + _a;
            HalfCarryFlag = ((_a & 0x0F) + (value & 0x0F)) > 0x0F;

            _a = (byte)(localA & 0xFF);

            CarryFlag = localA > 0xFF;
            SubstractFlag = false;
            ZeroFlag = _a == 0;
            _lastOpTime = 8;
        }

        protected void Sub(byte register)
        {
            _a -= register;
            HalfCarryFlag = (_a & 0x0F) == 0x0F; // Not sure how the Half carry flag is working
            _lastOpTime = 4;
            SubstractFlag = true;
            ZeroFlag = _a == 0;
        }

        protected void SubFromAddress(int address)
        {
            var value = _mmu.ReadByte(address);
            _a -= value;
            HalfCarryFlag = (_a & 0x0F) == 0x0F; // Not sure how the Half carry flag is working
            SubstractFlag = true;
            ZeroFlag = _a == 0;

            _lastOpTime = 8;
        }

        protected void SubFromImmediate()
        {
            var value = _mmu.ReadByte(PC++);
            _a -= value;
            HalfCarryFlag = (_a & 0x0F) == 0x0F; // Not sure how the Half carry flag is working
            SubstractFlag = true;
            ZeroFlag = _a == 0;

            _lastOpTime = 8;
        }

        protected void Call()
        {
            SP -= 2;
            _mmu.WriteWord(SP, PC + 2);
            PC = _mmu.ReadWord(PC);
            _lastOpTime = 12;
        }

        protected void Push(byte high, byte low)
        {
            _mmu.WriteByte(--SP, high);
            _mmu.WriteByte(--SP, low);
            _lastOpTime = 16;
        }

        protected void Pop(ref byte high, ref byte low)
        {
            low = _mmu.ReadByte(SP++);
            high = _mmu.ReadByte(SP++);

            _lastOpTime = 12;
        }

        protected ushort Pop()
        {
            var low = _mmu.ReadWord(SP);
            SP += 2;
            _lastOpTime = 12;
            return low;
        }

        protected void Return()
        {
            var address = Pop();
            PC = address;

            _lastOpTime = 8;
        }

        protected void RL(ref byte register)
        {
            bool carry = CarryFlag;

            CarryFlag = register >> 7 > 0;
            register = (byte)(register << 1);
            if (carry)
                register++;

            ZeroFlag = register == 0;
            SubstractFlag = false;
            HalfCarryFlag = false;

            _lastOpTime = 8;
        }

        protected void CP(byte register)
        {
            var value =_a - register;

            ZeroFlag = value == 0;
            SubstractFlag = true;
            HalfCarryFlag = (value & 0x0F) == 0x0F;
            CarryFlag = value < 0;

            _lastOpTime = 4;
        }

        protected void CP(ushort hl)
        {
            var register = _mmu.ReadByte(hl);
            var value = _a - register;

            ZeroFlag = value == 0;
            SubstractFlag = true;
            HalfCarryFlag = (value & 0x0F) == 0x0F;
            CarryFlag = value < 0;
            _lastOpTime = 8;

        }

        protected void CPImmediate()
        {
            var register = _mmu.ReadByte(PC++);
            var value = _a - register;

            ZeroFlag = value == 0;
            SubstractFlag = true;
            HalfCarryFlag = (value & 0x0F) == 0x0F;
            CarryFlag = value < 0;

            _lastOpTime = 8;

        }
    }
}
