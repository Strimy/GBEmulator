using Emulator.GB.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Emulator.GB.Core
{
    public partial class MMU : IMMU
    {
        byte[] _vram = new byte[0x2000];

        public byte ReadByte(int address)
        {
            if(address < 256)
            {
                return _bios[address];
            }

            throw new NotImplementedException("MMU failed at address "+ address);
        }

        public ushort ReadWord(int address)
        {
            var lowByte = ReadByte(address++);
            var highByte = ReadByte(address);

            return (ushort)((highByte << 8) + lowByte);
        }

        public void WriteByte(int address, byte value)
        {
            if(address >= 0x8000 && address < 0xA0000)
            {
                int shiftedAddress = address & 0x1FFF;
                _vram[shiftedAddress] = value;
            }
        }

        public void WriteWord(int address, int value)
        {
            throw new NotImplementedException();
        }
    }
}
